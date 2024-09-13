using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SliceAndDicePrototype.DiceSystem;
using SliceAndDicePrototype.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SliceAndDicePrototype.MatchMaking
{
    public class CompetitorView : MonoBehaviour
    {
        [SerializeField] private Image _competitorIcon;
        [SerializeField] private TextMeshProUGUI _competitorNameText;
        [SerializeField] private TextMeshProUGUI _armorText; 
        [SerializeField] private RectTransform _diceViewsContainer;
        [SerializeField] private SliderWithTextView _healthView;
        [SerializeField] private List<UIDieView> _diceViews;

        private Canvas _canvas;

        public List<UIDieView> DiceViews { get => _diceViews; }

        public void Initialize(Sprite icon, string name, List<UIDieView> diceViews)
        {
            _competitorIcon.sprite = icon;
            _competitorNameText.text = name;

            for (int i = 0; i < _diceViewsContainer.childCount; i++)
            {
                var destroyingDie = _diceViewsContainer.GetChild(i).GetComponent<UIDieView>();
                if (destroyingDie != null && !diceViews.Contains(destroyingDie))
                {
                    Destroy(_diceViewsContainer.GetChild(i).gameObject);
                }
            }

            _diceViews = diceViews;

            foreach (var diceView in diceViews)
            {
                diceView.transform.SetParent(_diceViewsContainer);
                diceView.ResetIconScale();
            }

            UIUtilities.RefreshUI(gameObject);
        }

        public void SetHealth(int health, int maxHealth)
        {
            _healthView.Initialize(health, maxHealth);
        }

        public void SetArmor(int armor)
        {
            _armorText.text = armor.ToString();
        }
    }
}