using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using SliceAndDicePrototype.Utilities;
using UnityEngine;

namespace SliceAndDicePrototype.DiceSystem
{
    public class DieView : MonoBehaviour
    {
        [Header("Physics")]
        [SerializeField] private Collider _collider;
        [SerializeField] private Rigidbody _rigidbody;
        [Tooltip("If the magnitude of the dice's motion vector is less than or equal to " +
            "this value, the dice will be considered motionless at this frame.")]
        [Range(0, 1)]
        [SerializeField] private float _restingForceThreshold = 0.001f;
        [Tooltip("Specifies the time interval in seconds after roll, " +
            "when the check for whether the dice is motionless will begin.")]
        [SerializeField] private float _motionlessCheckDelay = 1f;
        [Tooltip("Determines how many frames in a row the dice must be motionless " +
            "to complete the \"rolling\" process.")]
        [SerializeField] private int _motionlessDetectionFramesCount = 10;

        [Header("Sides")]
        [SerializeField] private Sides<DieSideView> _dieSides;

        private bool _isThrowed;

        public void Initialize(Sides<DieSideData> dieSidesData)
        {
            foreach (var sideWithData in _dieSides.GetSides())
            {
                sideWithData.Data.Initialize(dieSidesData.GetSide(sideWithData.Side));
            }
        }

        /// <summary>
        /// Physically throws the dice and waits for the result until the dice becomes motionless.
        /// </summary>
        /// <param name="force"></param>
        /// <returns>Top side of the dice after rolling process.</returns>
        public async Task<Side> Roll(Vector3 forceVector, CancellationToken cancellationToken)
        {
            if (_isThrowed || !IsMotionless())
            {
                throw new InvalidOperationException($"Rolling the die again is not allowed while it's moving!");
            }

            if (!IsPhysicsEnabled())
            {
                throw new InvalidOperationException($"Rolling the die is not allowed while physics disabled!");
            }

            _isThrowed = true;
            Bounds bounds = _collider.bounds;
            bounds.center = Vector3.zero;
            Vector3 randomPointInsideDie = bounds.GetRandomPointInside();
            Vector3 forcePoint = transform.TransformPoint(randomPointInsideDie);

            _rigidbody.AddForceAtPosition(forceVector, forcePoint);

            await UniTask.WaitForSeconds(
                _motionlessCheckDelay, cancellationToken: cancellationToken, cancelImmediately: true);
            CheckCancellation(cancellationToken);

            await WaitWhenBecomeMotionless(cancellationToken);

            _isThrowed = false;
            return GetTopSide();
        }

        /// <summary>
        /// <inheritdoc cref="Roll(Vector3, CancellationToken)"/>
        /// </summary>
        public async Task<Side> Roll(Vector3 forceVector)
        {
            return await Roll(forceVector, destroyCancellationToken);
        }

        public bool IsMotionless()
        {
            return _rigidbody.velocity.magnitude <= _restingForceThreshold;
        }

        public bool IsPhysicsEnabled()
        {
            return _rigidbody.isKinematic == false && _collider.enabled;
        }

        public void DisablePhysics()
        {
            if (!_rigidbody.isKinematic)
            {
                _rigidbody.velocity = Vector3.zero;
            }
            _rigidbody.isKinematic = true;
            _collider.enabled = false;
        }

        public void EnablePhysics()
        {
            _rigidbody.isKinematic = false;
            _collider.enabled = true;
        }

        public Side GetTopSide()
        {
            float maxDot = -Mathf.Infinity;
            Side topSide = Side.Up;

            foreach (var directionAndSide in SidesUtilities.SideNormals.GetSides())
            {
                Vector3 worldNormal = transform.TransformDirection(directionAndSide.Data);
                float dotProduct = Vector3.Dot(worldNormal, Vector3.up);

                if (dotProduct > maxDot)
                {
                    maxDot = dotProduct;
                    topSide = directionAndSide.Side;
                }
                
            }

            return topSide;
        }

        private void CheckCancellation(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _isThrowed = false;
                throw new OperationCanceledException();
            }
        }

        private async Task WaitWhenBecomeMotionless(CancellationToken cancellationToken)
        {
            int motionlessFrames = 0;
            while (motionlessFrames <= _motionlessDetectionFramesCount)
            {
                if (IsMotionless())
                {
                    motionlessFrames++;
                }

                if (gameObject.activeInHierarchy)
                {
                    await UniTask.WaitForEndOfFrame(this, cancellationToken, true);
                }
                CheckCancellation(cancellationToken);
            }
        }
    }
}
