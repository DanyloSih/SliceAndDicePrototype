using UnityEngine;
using UnityEngine.UI;

namespace SliceAndDicePrototype.Hacks
{
    public class HackWindowView : MonoBehaviour
    {
        [SerializeField] private RectTransform _hackWindow;
        [SerializeField] private Button _toggleVisibilityButton;

        protected void Start()
        {
            _toggleVisibilityButton.onClick.AddListener(OnClick);
        }

        protected void OnDestroy()
        {
            _toggleVisibilityButton.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            _hackWindow.gameObject.SetActive(!_hackWindow.gameObject.activeSelf);
        }
    }
}
