using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SliceAndDicePrototype.Utilities
{
    public class UIUtilities : MonoBehaviour
    {
        public static void RefreshUI(GameObject uiElement)
        {
            RectTransform[] allComponents = uiElement.GetComponentsInChildren<RectTransform>();

            foreach (var rectTransform in allComponents)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(uiElement.GetComponent<RectTransform>());
        }

        public static Vector3 GetUIElementWorldPoint(RectTransform uiElement, float height, Camera camera)
        {
            Plane plane = new Plane(Vector3.up, new Vector3(0, height, 0));
            Ray ray = camera.ScreenPointToRay(uiElement.position);
            Vector3 result = Vector3.zero;
            float distance;
            if (plane.Raycast(ray, out distance))
            {
                result = ray.GetPoint(distance);
            }

            return result;
        }
    } 
}
