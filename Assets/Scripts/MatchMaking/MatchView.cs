using System;
using SliceAndDicePrototype.DiceSystem;
using UnityEngine;

namespace SliceAndDicePrototype.MatchMaking
{
    public class MatchView
    {
        [Serializable]
        public struct Settings
        {
            public CompetitorView FirstCompetitorView;
            public CompetitorView SecondCompetitorView;
            public PopupView WinView;
            public PopupView LooseView;
        }

        private Settings _settings;
        private DiceTableView _diceTableView;

        public CompetitorView FirstCompetitorView { get => _settings.FirstCompetitorView; }
        public CompetitorView SecondCompetitorView { get => _settings.SecondCompetitorView; }
        public PopupView WinView { get => _settings.WinView; }
        public PopupView LooseView { get => _settings.LooseView; }
        public DiceTableView DiceTableView { get => _diceTableView; }

        public MatchView(
            Settings settings,
            DiceTableView diceTableView)
        {
            _settings = settings;
            _diceTableView = diceTableView;
        }
    }
}