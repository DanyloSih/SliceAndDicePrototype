using System.Collections.Generic;
using System.ComponentModel;
using SliceAndDicePrototype.DiceSystem;
using UnityEngine;
using Zenject;

namespace SliceAndDicePrototype.MatchMaking.Installers
{
    public class MatchViewInstaller : MonoInstaller
    {
        [SerializeField] private List<Transform> _randomPositionsPoolPoints;
        [SerializeField] private DieViewFactory.Settings _dieViewFactorySettings;
        [SerializeField] private DiceTableView.Settings _diceTableViewSettings;
        [SerializeField] private MatchView.Settings _matchViewSettings;

        public override void InstallBindings()
        {
            RandomPositionsPool randomPositionsPool = new RandomPositionsPool(_randomPositionsPoolPoints);
            Container.Bind<DieViewFactory>().AsSingle().WithArguments(_dieViewFactorySettings);

            Container.Bind<DiceTableView>().AsSingle()
                .WithArguments(_diceTableViewSettings, randomPositionsPool).WhenInjectedInto<MatchView>();

            Container.Bind<MatchView>().AsSingle().WithArguments(_matchViewSettings).NonLazy();
        }
    }

}