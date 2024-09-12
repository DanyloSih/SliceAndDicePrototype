using System;
using UnityEngine;

namespace SliceAndDicePrototype.DiceSystem
{
    [Serializable]
    public class Die
    {
        [SerializeField] private Side _activeSide = Side.Up;
        [SerializeField] private Sides<DieSide> _dieSides;

        public Sides<DieSide> DieSides { get => _dieSides; }
        public Side ActiveSide { get => _activeSide; }

        public Die(Sides<DieSide> dieSides, Side activeSide = Side.Up)
        {
            _dieSides = dieSides;
            _activeSide = activeSide;
        }
    }
}
