using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SliceAndDicePrototype.MatchMaking
{
    public class CompetitorView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _competitorName;
        [SerializeField] private TextMeshProUGUI _healthText;
        [SerializeField] private TextMeshProUGUI _armorText;
        [SerializeField] private Slider _healthBar;
        [SerializeField] private RectTransform _diceViewsContainer;
        [ShowNonSerializedField] private List<UIDieView> _diceViews;

        public List<UIDieView> DiceViews { get => _diceViews; }

        public void SetNewDiceViews(List<UIDieView> diceViews)
        {
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
                _diceViewsContainer.parent = diceView.transform;
            }
        }

        public void SetMaxHealth(float maxHealth)
        {
            _healthBar.maxValue = maxHealth;
        }

        public void SetHealth(int health)
        {
            _healthText.text = health.ToString();
            _healthBar.value = health;
        }

        public void SetArmor(int armor)
        {
            _armorText.text = armor.ToString();
        }
    }
}