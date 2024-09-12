using System.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;

namespace SliceAndDicePrototype.DiceSystem.ManualTesting
{
	public class DieViewTest : MonoBehaviour
	{
		[SerializeField] private DieView _dieViewPrefab;
		[Tooltip("min - Inclusive, max - Inclusive")]
		[SerializeField] private Vector2Int _minMaxLevel = new Vector2Int(1, 3);
		[SerializeField] private Vector2 _minMaxYForce = new Vector2(266, 400);
		[SerializeField] private Texture _testLeftTexture;
		[SerializeField] private Texture _testRightTexture;
		[SerializeField] private Texture _testUpTexture;
		[SerializeField] private Texture _testDownTexture;
		[SerializeField] private Texture _testForwardTexture;
		[SerializeField] private Texture _testBackTexture;

        private DieView _diceView;

        [Button]
		public void RespawnDice()
		{
			if (_diceView != null)
			{
				Destroy(_diceView.gameObject);
			}

			_diceView = Instantiate(_dieViewPrefab, transform);
			
			DiceSideData[] diceSidesData = new DiceSideData[6];
			diceSidesData[(int)DieSide.Left] = new DiceSideData(_testLeftTexture, GetRandomLevel());
			diceSidesData[(int)DieSide.Right] = new DiceSideData(_testRightTexture, GetRandomLevel());
			diceSidesData[(int)DieSide.Up] = new DiceSideData(_testUpTexture, GetRandomLevel());
			diceSidesData[(int)DieSide.Down] = new DiceSideData(_testDownTexture, GetRandomLevel());
			diceSidesData[(int)DieSide.Forward] = new DiceSideData(_testForwardTexture, GetRandomLevel());
			diceSidesData[(int)DieSide.Back] = new DiceSideData(_testBackTexture, GetRandomLevel());

            _diceView.Initialize(diceSidesData);
            _diceView.transform.position = transform.position;
        }

        [Button]
        public async void ThrowDice()
		{
            if (_diceView == null)
			{
				return;
			}
			Task<DieSide> throwTask = _diceView.Roll(GetThrowForce());
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