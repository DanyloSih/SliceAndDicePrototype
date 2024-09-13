using UnityEngine;

namespace SliceAndDicePrototype.DiceSystem
{
    public abstract class DieSide : ScriptableObject
    {
        [SerializeField] private Texture _icon;
        [Min(1)]
        [SerializeField] private int _level;

        public int Level { get => _level; }

        public abstract void UseAbility();

        public DieSideData GetSideData()
        {
            return new DieSideData(_icon, _level);
        }
    }
}
