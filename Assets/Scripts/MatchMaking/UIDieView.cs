using DG.Tweening;
using NaughtyAttributes;
using SliceAndDicePrototype.DiceSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SliceAndDicePrototype.MatchMaking
{
    public class UIDieView : MonoBehaviour
    {
        private static readonly Vector3 s_hideIconSize;
        private Tween _hidingTween;
        private Tween _showingTween;

        [SerializeField] private Image _iconImage;
        [SerializeField] private Image _selectionImage;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private Vector3 _initialIconScale;
        [SerializeField] private float _hideIconDuration = 1f;
        [SerializeField] private float _showIconDuration = 1f;

        public void Initialize(DieSideData dieSideData)
        {
            _iconImage.sprite = dieSideData.IconSprite;
            _levelText.text = dieSideData.Level.ToString();
            _iconImage.rectTransform.localScale = _initialIconScale;
        }

        [Button]
        public void Select()
        {
            _selectionImage.enabled = true;
        }

        [Button]
        public void Unselect()
        {
            _selectionImage.enabled = false;
        }

        [Button]
        public Tween HideIcon()
        {
            InitializeHidingTween();
            _hidingTween.Restart();
            _hidingTween.Play();
            return _hidingTween;
        }

        [Button]
        public Tween ShowIcon()
        {
            InitializeShowingTween();
            _showingTween.Restart();
            _showingTween.Play();
            return _showingTween;
        }

        private void InitializeShowingTween()
        {
            if (_showingTween.IsActive())
            {
                return;
            }

            _showingTween = _iconImage.rectTransform.DOScale(_initialIconScale, _showIconDuration)
                .OnStart(() =>
                {
                    _iconImage.rectTransform.localScale = s_hideIconSize;
                    _iconImage.enabled = true;
                    _levelText.enabled = true;
                });

            _showingTween.Pause();
        }

        private void InitializeHidingTween()
        {
            if (_hidingTween.IsActive())
            {
                return;
            }

            _hidingTween = _iconImage.rectTransform.DOScale(s_hideIconSize, _hideIconDuration)
                .OnStart(() =>
                {
                    _iconImage.rectTransform.localScale = _initialIconScale;
                    _iconImage.enabled = true;
                    _levelText.enabled = true;
                });
            _hidingTween.onComplete += OnHide;

            _hidingTween.Pause();
        }

        private void OnHide()
        {
            _iconImage.enabled = false;
            _levelText.enabled = false;
        }
    }
}