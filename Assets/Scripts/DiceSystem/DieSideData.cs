using System;
using System.Collections;
using UnityEngine;

namespace SliceAndDicePrototype.DiceSystem
{
    public struct DieSideData
    {
        public readonly Texture Icon;
        public readonly Sprite IconSprite;
        public readonly int Level;

        public DieSideData(Texture icon, int level)
        {
            if (icon == null)
            {
                throw new ArgumentNullException(nameof(icon));
            }

            if (icon is Texture2D)
            {
                IconSprite = Sprite.Create(
                    (Texture2D)icon, new Rect(0, 0, icon.width, icon.height), Vector2.one / 2f);
            }
            else
            {
                throw new ArgumentException($"Icon shold be 2D texture!", nameof(icon));
            }

            if (level <= 0)
            {
                throw new ArgumentException($"Should be greater then 0!", nameof(level));
            }

            Icon = icon;
            Level = level;
        }
    }
}
