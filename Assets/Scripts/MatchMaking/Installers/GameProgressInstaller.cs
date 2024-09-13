using Zenject;

namespace SliceAndDicePrototype.MatchMaking.Installers
{
    public class GameProgressInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<GameProgress>().AsSingle();
        }
    }

}