namespace SliceAndDicePrototype.MatchMaking
{
    public class CompetitorsPair
    {
        private Competitor _firstCompetitor;
        private Competitor _secondCompetitor;
        private CompetitorBehaviour _firstCompetitorBehaviour;
        private CompetitorBehaviour _secondCompetitorBehaviour;

        public Competitor FirstCompetitor { get => _firstCompetitor; }
        public Competitor SecondCompetitor { get => _secondCompetitor; }
        public CompetitorBehaviour FirstCompetitorBehaviour { get => _firstCompetitorBehaviour; }
        public CompetitorBehaviour SecondCompetitorBehaviour { get => _secondCompetitorBehaviour; }

        public CompetitorsPair(
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
}