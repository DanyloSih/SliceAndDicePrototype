using System;
using System.Threading.Tasks;
using SimpleBootstrap;
using SliceAndDicePrototype.MatchMaking;
using Zenject;

namespace SliceAndDicePrototype.Bootstrappers
{
    public class MatchBootstrapper : AsyncBootstrapScript, IMatchProvider
    {
        [Inject] private MatchFactory _matchFactory;
        [Inject] private GameProgress _gameProgress;

        public Match Match { get; private set; }

        protected async override Task OnRunAsync(BootstrapContext bootstrapContext)
        {
            try
            {
                while (true)
                {
                    _gameProgress.CurrentLevel = _gameProgress.CurrentLevel % _matchFactory.LevelsCount;
                    Match = _matchFactory.Create(_gameProgress.CurrentLevel);
                    MatchResult result = await Match.StartMatch(destroyCancellationToken);
                    if (result == MatchResult.Win)
                    {
                        _gameProgress.CurrentLevel++;
                    }
                }
            }
            catch (OperationCanceledException)
            {

            }
        }
    } 
}
