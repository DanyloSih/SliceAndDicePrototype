using SliceAndDicePrototype.MatchMaking;
using UnityEngine;
using Zenject;

namespace SliceAndDicePrototype.Bootstrappers.Installers
{
    public class MatchProviderInstaller : MonoInstaller
    {
        [SerializeField] private MatchBootstrapper _matchBootstrapper;

        public override void InstallBindings()
        {
            Container.Bind<IMatchProvider>().FromInstance(_matchBootstrapper).AsSingle();       
        }
    }
}
