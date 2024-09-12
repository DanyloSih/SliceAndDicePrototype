using UnityEngine;

namespace SliceAndDicePrototype.DiceSystem
{
    public struct DiceSideData
    {
        public readonly Texture Icon;
        public readonly int Level;

        public DiceSideData(Texture icon, int level)
        {
            if (icon == null)
            {
                throw new System.ArgumentNullException(nameof(icon));
            }

            if (level <= 0)
            {
                throw new System.ArgumentException($"Should be greater then 0!", nameof(level));
            }

            Icon = icon;
            Level = level;
        }
    }
}
