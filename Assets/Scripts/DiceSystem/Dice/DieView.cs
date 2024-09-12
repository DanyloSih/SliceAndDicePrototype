using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SliceAndDicePrototype.DiceSystem
{
    public class DieView : MonoBehaviour
    {
        private static readonly Vector3[] s_sideNormals = new Vector3[]
        {
            Vector3.left,   
            Vector3.right,  
            Vector3.up,     
            Vector3.down,   
            Vector3.forward,
            Vector3.back    
        };

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
        [SerializeField] private DieSideView _sideLeft;
        [SerializeField] private DieSideView _sideRight;
        [SerializeField] private DieSideView _sideUp;
        [SerializeField] private DieSideView _sideDown;
        [SerializeField] private DieSideView _sideForward;
        [SerializeField] private DieSideView _sideBack;
        private bool _isThrowed;

        /// <summary>
        /// The order of the sides in <paramref name="diceSidesData"/> must 
        /// match the order of the sides defined in <see cref="DieSide"/> enum.
        /// </summary>
        /// <param name="diceSidesData"></param>
        public void Initialize(DiceSideData[] diceSidesData)
        {
            _sideLeft.Initialize(diceSidesData[(int)DieSide.Left]);
            _sideRight.Initialize(diceSidesData[(int)DieSide.Right]);
            _sideUp.Initialize(diceSidesData[(int)DieSide.Up]);
            _sideDown.Initialize(diceSidesData[(int)DieSide.Down]);
            _sideForward.Initialize(diceSidesData[(int)DieSide.Forward]);
            _sideBack.Initialize(diceSidesData[(int)DieSide.Back]);
        }

        /// <summary>
        /// Physically throws the dice and waits for the result until the dice becomes motionless.
        /// </summary>
        /// <param name="force"></param>
        /// <returns>Top side of the dice after rolling process.</returns>
        public async Task<DieSide> Roll(Vector3 forceVector, CancellationToken cancellationToken)
        {
            if (_isThrowed || !IsMotionless())
            {
                throw new InvalidOperationException($"Rolling the dice again is not allowed while it's moving!");
            }

            _isThrowed = true;
            Bounds bounds = _collider.bounds;
            bounds.center = Vector3.zero;
            Vector3 randomPointInsideDie = bounds.GetRandomPointInside();
            Vector3 forcePoint = transform.TransformPoint(randomPointInsideDie);

            _rigidbody.AddForceAtPosition(forceVector, forcePoint);

            await UniTask.WaitForSeconds(
                _motionlessCheckDelay, cancellationToken: cancellationToken, cancelImmediately: true);
            if (cancellationToken.IsCancellationRequested)
            {
                _isThrowed = false;
                throw new OperationCanceledException();
            }

            int motionlessFrames = 0;
            while (motionlessFrames <= _motionlessDetectionFramesCount)
            {
                if(IsMotionless())
                {
                    motionlessFrames++;
                }

                await UniTask.WaitForEndOfFrame(this, cancellationToken, true);
                if (cancellationToken.IsCancellationRequested)
                {
                    _isThrowed = false;
                    throw new OperationCanceledException();
                }
            }

            _isThrowed = false;
            return GetCurrentTopDiceSide();
        }

        /// <summary>
        /// <inheritdoc cref="Roll(Vector3, CancellationToken)"/>
        /// </summary>
        public async Task<DieSide> Roll(Vector3 forceVector)
        {
            return await Roll(forceVector, destroyCancellationToken);
        }

        public bool IsMotionless()
        {
            return _rigidbody.velocity.magnitude <= _restingForceThreshold;
        }

        public void DisablePhysics()
        {
            _rigidbody.isKinematic = true;
            _rigidbody.velocity = Vector3.zero;
            _collider.enabled = false;
        }

        public void EnablePhysics()
        {
            _rigidbody.isKinematic = false;
            _collider.enabled = true;
        }

        private DieSide GetCurrentTopDiceSide()
        {
            float maxDot = -Mathf.Infinity;
            DieSide topSide = DieSide.Up;

            for (int i = 0; i < s_sideNormals.Length; i++)
            {
                Vector3 worldNormal = transform.TransformDirection(s_sideNormals[i]);
                float dotProduct = Vector3.Dot(worldNormal, Vector3.up);

                if (dotProduct > maxDot)
                {
                    maxDot = dotProduct;
                    topSide = (DieSide)i;
                }
            }

            return topSide;
        }
    }
}
