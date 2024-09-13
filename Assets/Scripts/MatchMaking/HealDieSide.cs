using SliceAndDicePrototype.DiceSystem;
using UnityEngine;
using Zenject;

namespace SliceAndDicePrototype.MatchMaking
{
    [CreateAssetMenu(
        fileName = nameof(HealDieSide),
        menuName = "SliceAndDicePrototype/Dice/" + nameof(HealDieSide))]
    public class HealDieSide : DieSide
    {
        [Inject]
        private IMatchProvider _matchProvider;

        public override void UseAbility()
        {
            _matchProvider.Match.PlayingCompetitor.AddHealth(Level);
        }
    }
}