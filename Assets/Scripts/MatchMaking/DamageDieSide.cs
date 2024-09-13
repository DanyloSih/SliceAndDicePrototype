using SliceAndDicePrototype.DiceSystem;
using UnityEngine;
using Zenject;

namespace SliceAndDicePrototype.MatchMaking
{
    [CreateAssetMenu(
        fileName = nameof(DamageDieSide),
        menuName = "SliceAndDicePrototype/Dice/" + nameof(DamageDieSide))]
    public class DamageDieSide : DieSide
    {
        [Inject]
        private IMatchProvider _matchProvider;

        public override void UseAbility()
        {
            _matchProvider.Match.WaitingCompetitor.DealDamage(Level);
        }
    }
}