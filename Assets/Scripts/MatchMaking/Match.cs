using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using SliceAndDicePrototype.DiceSystem;
using UnityEngine;

namespace SliceAndDicePrototype.MatchMaking
{
    public class Match
    {
        [Serializable]
        public struct Settings
        {
            public float ApplySideDelay;
            public float DelayBeforeRoll;
            public float DelayAfterRoll;
            public Vector2 MinMaxYRollForce;
        }

        private readonly Settings _settings;
        private readonly int _currentLevel;
        private readonly int _levelsCount;
        private CompetitorsPair _competitorsPair;
        private MatchView _matchView;
        private UIDiceViewsFactory _uIDiceViewsFactory;

        private int _move = 0;
        private bool _isStarted;

        public CompetitorsPair CompetitorsPair { get => _competitorsPair; }
        public Competitor PlayingCompetitor => _move % 2 == 0 ? _competitorsPair.FirstCompetitor : _competitorsPair.SecondCompetitor;
        public Competitor WaitingCompetitor => _move % 2 == 0 ? _competitorsPair.SecondCompetitor : _competitorsPair.FirstCompetitor;
        public MatchView MatchView { get => _matchView; }

        public Match(
            Settings settings,
            int currentLevel,
            int levelsCount,
            CompetitorsPair competitorsPair,
            MatchView matchUI,
            UIDiceViewsFactory uIDiceViewsFactory)
        {
            _settings = settings;
            _currentLevel = currentLevel;
            _levelsCount = levelsCount;
            _competitorsPair = competitorsPair;
            _matchView = matchUI;

            _uIDiceViewsFactory = uIDiceViewsFactory;
            _matchView.FirstCompetitorView.Initialize(
                _competitorsPair.FirstCompetitor.Icon,
                _competitorsPair.FirstCompetitor.Name,
                _uIDiceViewsFactory.Create(_competitorsPair.FirstCompetitor.Dice));

            _matchView.SecondCompetitorView.Initialize(
                _competitorsPair.SecondCompetitor.Icon,
                _competitorsPair.SecondCompetitor.Name,
                _uIDiceViewsFactory.Create(_competitorsPair.SecondCompetitor.Dice));

            _matchView.WinView.Initialize(_currentLevel + 1, _levelsCount);
            _matchView.LooseView.Initialize(_currentLevel + 1, _levelsCount);

            UpdateUI();
        }

        public async Task<MatchResult> StartMatch(CancellationToken cancellationToken)
        {
            if (_isStarted)
            {
                throw new InvalidOperationException();
            }

            _isStarted = true;
            _matchView.WinView.gameObject.SetActive(false);
            _matchView.LooseView.gameObject.SetActive(false);

            while (_competitorsPair.FirstCompetitor.Health != 0 && _competitorsPair.SecondCompetitor.Health != 0)
            {
                await StartNextMove(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
            }

            if (_competitorsPair.FirstCompetitor.Health == 0)
            {
                await WaitForPopUp(_matchView.WinView, cancellationToken);
                _isStarted = false;
                return MatchResult.Win;
            }
            else
            {
                await WaitForPopUp(_matchView.LooseView, cancellationToken);
                _isStarted = false;
                return MatchResult.Loose;
            }
        }

        public void UpdateUI()
        {
            UpdateUI(_matchView.FirstCompetitorView, _competitorsPair.FirstCompetitor);
            UpdateUI(_matchView.SecondCompetitorView, _competitorsPair.SecondCompetitor);
        }

        private async Task WaitForPopUp(PopupView popupView, CancellationToken cancellationToken)
        {
            popupView.gameObject.SetActive(true);
            await popupView.WaitForPlayerResponse(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            popupView.gameObject.SetActive(false);
        }

        private Task StartNextMove(CancellationToken cancellationToken)
        {
            if (_move % 2 == 0)
            {
                return StartMove(
                    _matchView.FirstCompetitorView,
                    _competitorsPair.FirstCompetitor,
                    _competitorsPair.FirstCompetitorBehaviour,
                    cancellationToken);
            }
            else
            {
                return StartMove(
                    _matchView.SecondCompetitorView,
                    _competitorsPair.SecondCompetitor,
                    _competitorsPair.SecondCompetitorBehaviour,
                    cancellationToken);
            }
        }

        private async Task StartMove(CompetitorView competitorView, Competitor competitor, CompetitorBehaviour competitorBehaviour, CancellationToken cancellationToken)
        {
            await competitorBehaviour.WaitToAction(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            RegisterCompetitorInDiceTable(competitorView, competitor);

            await _matchView.DiceTableView.MoveDiceViewsToCenter(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();

            await UniTask.WaitForSeconds(_settings.DelayBeforeRoll, cancellationToken: cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();

            await _matchView.DiceTableView.RollDice(GetThrowForce(), cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();

            await UniTask.WaitForSeconds(_settings.DelayAfterRoll, cancellationToken: cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();

            UpdateDiceSides(competitorView, competitor);

            await _matchView.DiceTableView.MoveDiceViewsToRelatedUIElement(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();

            await ApplySidesEffects(competitorView, competitor, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            UpdateUI();
            _move++;
        }

        private async Task ApplySidesEffects(CompetitorView competitorView, Competitor competitor, CancellationToken cancellationToken)
        {
            int counter = 0;
            foreach (var die in competitor.Dice)
            {
                competitorView.DiceViews[counter].Select();
                die.DieSides.GetSide(die.TopSide).UseAbility();
                UpdateUI();
                await UniTask.WaitForSeconds(_settings.ApplySideDelay, cancellationToken: cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                competitorView.DiceViews[counter].Unselect();
                counter++;
            }
        }

        private void UpdateDiceSides(CompetitorView competitorView, Competitor competitor)
        {
            int counter = 0;
            var newSides = _matchView.DiceTableView.GetSides();
            foreach (var uiDieView in competitorView.DiceViews)
            {
                Side newSide = newSides[uiDieView];
                competitor.Dice[counter].TopSide = newSide;
                counter++;
            }
        }

        private Vector3 GetThrowForce()
        {
            return new Vector3(0, UnityEngine.Random.Range(_settings.MinMaxYRollForce.x, _settings.MinMaxYRollForce.y), 0);
        }

        private void RegisterCompetitorInDiceTable(CompetitorView competitorView, Competitor competitor)
        {
            _matchView.DiceTableView.UnregisterAll();

            int counter = 0;
            foreach (var UIDieView in competitorView.DiceViews)
            {
                Die die = competitor.Dice[counter];
                _matchView.DiceTableView.RegisterDie(
                    UIDieView, die.TopSide, die.DieSides.ConvertToNewSidesType((a, b) => a.GetSideData()));

                counter++;
            }
        }

        private void UpdateUI(CompetitorView view, Competitor competitor)
        {
            view.SetArmor(competitor.Armor);
            view.SetHealth(competitor.Health, competitor.MaxHealth);
        }
    }
}