using SliceAndDicePrototype.DiceSystem;
using UnityEngine;

namespace SliceAndDicePrototype.MatchMaking
{
    [CreateAssetMenu(
        fileName = nameof(DieProvider),
        menuName = "SliceAndDicePrototype/Dice/" + nameof(DieProvider))]
    public class DieProvider : ScriptableObject
    {
        [SerializeField] private Sides<DieSide> _dieSides;
        [SerializeField] private Side _topSide = Side.Up;

        public Die GetDie()
        {
            return new Die(_topSide, _dieSides);
        }
    }
}