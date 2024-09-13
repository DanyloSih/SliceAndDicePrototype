using SliceAndDicePrototype.DiceSystem;
using UnityEngine;
using Zenject;

namespace SliceAndDicePrototype.MatchMaking.Installers
{

    public class MatchFactoryInstaller : MonoInstaller
    {
        [SerializeField] private Match.Settings _matchSettings;
        [SerializeField] private CompetitorsPairsFactory.Settings _competitorsPairsFactorySettings;
        [SerializeField] private UIDieView _uiDieViewPreafab;

        public override void InstallBindings()
        {
            Container.Bind<CompetitorsPairsFactory>().AsSingle().WithArguments(_competitorsPairsFactorySettings);
            Container.Bind<UIDiceViewsFactory>().AsSingle().WithArguments(_uiDieViewPreafab);
            Container.Bind<MatchFactory>().AsSingle().WithArguments(
                _matchSettings, _competitorsPairsFactorySettings.EnemiesCompetitorProviders.Count).NonLazy();
        }
    }

}