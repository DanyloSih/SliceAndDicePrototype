using System.Collections.Generic;
using System.Linq;
using SliceAndDicePrototype.DiceSystem;
using UnityEngine;

namespace SliceAndDicePrototype.MatchMaking
{
    [CreateAssetMenu(
        fileName = nameof(CompetitorProvider),
        menuName = "SliceAndDicePrototype/MatchMaking/" + nameof(CompetitorProvider))]
    public class CompetitorProvider : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private Sprite _icon;
        [SerializeField] private List<DieProvider> _diceProviders;
        [SerializeField] private int _maxHealth = 10;
        [SerializeField] private int _initialHealth = 10;
        [SerializeField] private int _initialArmor = 0;
        [SerializeField] private CompetitorBehaviour _competitorBehaviour;

        public Competitor GetCompetitor()
        {
            List<Die> dice = _diceProviders.Select((a) => a.GetDie()).ToList();
            Competitor competitor = new Competitor(_name, _icon, dice, _maxHealth, _initialHealth, _initialArmor);
            return competitor;
        }

        public CompetitorBehaviour GetCompetitorBehaviour()
        {
            return _competitorBehaviour;
        }
    }
}