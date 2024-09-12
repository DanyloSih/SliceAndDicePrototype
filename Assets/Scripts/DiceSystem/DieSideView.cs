using TMPro;
using UnityEngine;

namespace SliceAndDicePrototype.DiceSystem
{
    public class DieSideView : MonoBehaviour
    {
        [SerializeField] private Renderer _iconMaterialRenderer;
        [SerializeField] private TextMeshPro _levelText;

        private Material _iconMaterial;

        public void Initialize(DieSideData diceSideData)
        {
            _iconMaterial = _iconMaterialRenderer.material;
            _iconMaterial.SetTexture("_MainTex", diceSideData.Icon);
            _levelText.text = diceSideData.Level.ToString();
        }
    } 
}
