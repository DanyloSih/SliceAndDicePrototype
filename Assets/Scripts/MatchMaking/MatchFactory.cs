using SliceAndDicePrototype.DiceSystem;

namespace SliceAndDicePrototype.MatchMaking
{
    public class MatchFactory
    {
        private Match.Settings _matchSettings;
        private MatchView _matchView;
        private CompetitorsPairsFactory _competitorsPairsProvider;
        private UIDiceViewsFactory _uiDiceViewFactory;
        private readonly int _levelsCount;

        public int LevelsCount => _levelsCount;

        public MatchFactory(
            Match.Settings matchSettings,
            MatchView matchView,
            CompetitorsPairsFactory competitorsPairsProvider,
            UIDiceViewsFactory uiDiceViewFactory,
            int levelsCount)
        {
            _matchSettings = matchSettings;
            _matchView = matchView;
            _competitorsPairsProvider = competitorsPairsProvider;
            _uiDiceViewFactory = uiDiceViewFactory;
            _levelsCount = levelsCount;
        }

        public Match Create(int levelId)
        {
            return new Match(
                _matchSettings, 
                levelId,
                _levelsCount,
                _competitorsPairsProvider.GetCompetitorsPair(levelId),
                _matchView,
                _uiDiceViewFactory);
        }
    }
}