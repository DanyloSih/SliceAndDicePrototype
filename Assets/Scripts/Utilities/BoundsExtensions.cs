using UnityEngine;

namespace SliceAndDicePrototype.Utilities
{
    public static class BoundsExtensions
    {
        public static Vector3 GetRandomPointInside(this Bounds bounds)
        {
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float y = Random.Range(bounds.min.y, bounds.max.y);
            float z = Random.Range(bounds.min.z, bounds.max.z);
            return new Vector3(x, y, z);
        }
    }
}
