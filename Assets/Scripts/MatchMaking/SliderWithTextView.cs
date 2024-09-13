using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SliceAndDicePrototype.MatchMaking
{
    public class SliderWithTextView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Slider _progressBar;
        [Tooltip("{0} is placeholder for current value, {1} is placeholder for max value.")]
        [SerializeField] private string _textFormat = "{0}/{1}";

        public void Initialize(float currentValue, float maxValue)
        {
            _progressBar.maxValue = maxValue;
            _progressBar.value = currentValue;
            _text.text = string.Format(_textFormat, currentValue, maxValue);
        }
    }
}