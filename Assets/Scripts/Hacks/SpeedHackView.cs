using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SliceAndDicePrototype.Hacks
{
    public class SpeedHackView : MonoBehaviour
    {
        [SerializeField] private Slider _ammountSlider;
        [SerializeField] private TextMeshProUGUI _ammountText;

        protected void Update()
        {
            _ammountText.text = _ammountSlider.value.ToString();
            Time.timeScale = _ammountSlider.value;
        }
    }
}
