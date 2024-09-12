using System;
using System.Collections.Generic;
using NaughtyAttributes;
using SliceAndDicePrototype.DiceSystem;
using UnityEngine;

namespace SliceAndDicePrototype.MatchMaking
{
    public class Match : IRoundProvider
    {
        private Competitor _firstCompetitor;
        private Competitor _secondCompetitor;
        private CompetitorBehaviour _firstCompetitorBehaviour;
        private CompetitorBehaviour _secondCompetitorBehaviour;

        public int Round { get; }

        public Match(
            Competitor firstCompetitor,
            CompetitorBehaviour firstCompetitorBehaviour,
            Competitor secondCompetitor,
            CompetitorBehaviour secondCompetitorBehaviour)
        {
            _firstCompetitor = firstCompetitor;
            _firstCompetitorBehaviour = firstCompetitorBehaviour;
            _secondCompetitor = secondCompetitor;
            _secondCompetitorBehaviour = secondCompetitorBehaviour;
        }

        
    }

    [Serializable]
    public class Competitor
    {
        [SerializeField] private string _name;
        [SerializeField] private Texture _icon;
        [SerializeField] private List<Die> _dice;
        [SerializeField] private int _maxHealth = 10;
        [ShowNonSerializedField] private int _health = 10;

        public int Health { get => _health; set => Mathf.Clamp(value, 0, _maxHealth); }
        public List<Die> Dice { get => _dice; }
        public Texture Icon { get => _icon; }
        public string Name { get => _name; }

        public Competitor(string name, Texture icon, List<Die> dice, int maxHealth)
        {
            _name = name;
            _icon = icon;
            _dice = dice;
            _maxHealth = maxHealth;
        }
    }
}