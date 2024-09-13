using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SliceAndDicePrototype.MatchMaking
{
    public class CompetitorsPairsFactory
    {
        [Serializable]
        public struct Settings
        {
            public CompetitorProvider PlayerCompetitorProvider;
            public List<CompetitorProvider> EnemiesCompetitorProviders;
        }

        private DiContainer _diContainer;
        private Settings _settings;

        public CompetitorsPairsFactory(Settings settings, DiContainer diContainer)
        {
            _settings = settings;
            _diContainer = diContainer;
        }

        public int GetCompetitorsCount()
        {
            return _settings.EnemiesCompetitorProviders.Count;
        }

        public CompetitorsPair GetCompetitorsPair(int competitorId)
        {
            var firstCompetitorProvider = _settings.EnemiesCompetitorProviders[competitorId];
            var pairs = new CompetitorsPair(
                firstCompetitorProvider.GetCompetitor(),
                firstCompetitorProvider.GetCompetitorBehaviour(),
                _settings.PlayerCompetitorProvider.GetCompetitor(),
                _settings.PlayerCompetitorProvider.GetCompetitorBehaviour());

            InjectCompetitorDice(pairs.FirstCompetitor);
            InjectCompetitorDice(pairs.SecondCompetitor);

            return pairs;
        }

        private void InjectCompetitorDice(Competitor competitor)
        {
            foreach (var die in competitor.Dice)
            {
                foreach (DiceSystem.SideWithData<DiceSystem.DieSide> dieSide in die.DieSides.GetSides())
                {
                    _diContainer.Inject(dieSide.Data);
                }
            }
        }
    }
}