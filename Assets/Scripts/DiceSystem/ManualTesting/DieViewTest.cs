using System.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;

namespace SliceAndDicePrototype.DiceSystem.ManualTesting
{
    public class DieViewTest : MonoBehaviour
	{
		[SerializeField] private DieView _dieViewPrefab;
		[Tooltip("min - Inclusive, max - Inclusive"), Min(1)]
		[SerializeField] private Vector2Int _minMaxLevel = new Vector2Int(1, 3);
        [Tooltip("min - Inclusive, max - Inclusive")]
        [SerializeField] private Vector2 _minMaxYForce = new Vector2(266, 400);
		[SerializeField] private Sides<Texture> _testSidesTextures;

        private DieView _dieView;

        [Button]
		public void RespawnDice()
		{
			if (_dieView != null)
			{
				Destroy(_dieView.gameObject);
			}

            Sides<DieSideData> dieSidesData = _testSidesTextures.ConvertToNewSidesType(
				(a, b) => new DieSideData(a, GetRandomLevel()));

            _dieView = Instantiate(_dieViewPrefab, transform);
            _dieView.Initialize(dieSidesData);
            _dieView.transform.position = transform.position;
        }

        [Button]
        public async void ThrowDice()
		{
            if (_dieView == null)
			{
				return;
			}
			Task<Side> throwTask = _dieView.Roll(GetThrowForce());
			await throwTask;

			if(throwTask.IsCompletedSuccessfully)
			{
                Debug.Log($"Throwing task completed with result: {throwTask.Result}!");
            }
			else
			{
				Debug.LogWarning($"Throwing task completed unsuccessfully!");
			}
        }

        [Button]
        public void EnablePhysics()
		{
            if (_dieView != null)
            {
                _dieView.EnablePhysics();
            }
        }

        [Button]
        public void DisablePhysics()
        {
            if (_dieView != null)
            {
                _dieView.DisablePhysics();
            }
        }

        [Button]
        public void SetNextTopSide()
        {
			if (_dieView != null)
			{
                Side side = (Side)(((int)_dieView.GetTopSide() + 1) % 6);
                Debug.Log($"{side} side was set.");
                Quaternion targetRotation = SidesUtilities.SideRotations.GetSide(side);
                _dieView.transform.rotation = targetRotation;
			}
        }

        private Vector3 GetThrowForce()
		{
			return new Vector3(0, Random.Range(_minMaxYForce.x, _minMaxYForce.y), 0);
		}

		private int GetRandomLevel()
		{
			return Random.Range(_minMaxLevel.x, _minMaxLevel.y + 1);
		}
	}

}