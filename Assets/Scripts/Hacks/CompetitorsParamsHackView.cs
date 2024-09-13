using System;
using System.Collections.Generic;
using System.Linq;
using SliceAndDicePrototype.MatchMaking;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SliceAndDicePrototype.Hacks
{
    public class CompetitorsParamsHackView : MonoBehaviour
    {
        private readonly Dictionary<string, Action<Competitor, int>> s_addActions = new() {
        { "armor", AddArmor },
        { "health", AddHealth },
    };

        private readonly Dictionary<string, Action<Competitor, int>> s_removeActions = new() {
        { "armor", RemoveArmor },
        { "health", RemoveHealth },
    };

        [Inject] private IMatchProvider _matchProvider;

        [SerializeField] private Button _addButton;
        [SerializeField] private Button _removeButton;
        [SerializeField] private TextMeshProUGUI _ammountText;
        [SerializeField] private Slider _ammountSlider;
        [SerializeField] private TMP_Dropdown _actionDropdown;
        [SerializeField] private TMP_Dropdown _competitorDropdown;

        public Match Match => _matchProvider.Match;

        protected void OnEnable()
        {
            _competitorDropdown.ClearOptions();
            _competitorDropdown.AddOptions(new List<string>() {
            Match.CompetitorsPair.FirstCompetitor.Name,
            Match.CompetitorsPair.SecondCompetitor.Name
        });

            _actionDropdown.ClearOptions();
            _actionDropdown.AddOptions(
                s_addActions.Select((a) => a.Key).ToList());

            _addButton.onClick.AddListener(OnAdd);
            _removeButton.onClick.AddListener(OnRemove);
        }

        protected void OnDisable()
        {
            _addButton.onClick.RemoveListener(OnAdd);
            _removeButton.onClick.RemoveListener(OnRemove);
        }

        protected void Update()
        {
            _ammountText.text = _ammountSlider.value.ToString();
        }

        private void OnAdd()
        {
            s_addActions[_actionDropdown.options[_actionDropdown.value].text]
                .Invoke(GetCompetitor(), (int)_ammountSlider.value);

            Match.UpdateUI();
        }

        private void OnRemove()
        {
            s_removeActions[_actionDropdown.options[_actionDropdown.value].text]
               .Invoke(GetCompetitor(), (int)_ammountSlider.value);

            Match.UpdateUI();
        }

        private Competitor GetCompetitor()
        {
            string targetName = _competitorDropdown.options[_competitorDropdown.value].text;
            if (targetName.Equals(Match.CompetitorsPair.FirstCompetitor.Name))
            {
                return Match.CompetitorsPair.FirstCompetitor;
            }
            else
            {
                return Match.CompetitorsPair.SecondCompetitor;
            }
        }

        private static void AddHealth(Competitor competitor, int value)
        {
            competitor.AddHealth((int)value);
        }

        private static void AddArmor(Competitor competitor, int value)
        {
            competitor.AddArmor((int)value);
        }

        private static void RemoveHealth(Competitor competitor, int value)
        {
            competitor.HitHealth((int)value);
        }

        private static void RemoveArmor(Competitor competitor, int value)
        {
            competitor.HitArmor((int)value);
        }
    } 
}
