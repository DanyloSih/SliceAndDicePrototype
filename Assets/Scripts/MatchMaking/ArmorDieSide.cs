using SliceAndDicePrototype.DiceSystem;
using UnityEngine;
using Zenject;

namespace SliceAndDicePrototype.MatchMaking
{
    [CreateAssetMenu(
        fileName = nameof(ArmorDieSide),
        menuName = "SliceAndDicePrototype/Dice/" + nameof(ArmorDieSide))]
    public class ArmorDieSide : DieSide
    {
        [Inject]
        private IMatchProvider _matchProvider;

        public override void UseAbility()
        {
            _matchProvider.Match.PlayingCompetitor.AddArmor(Level);
        }
    }
}