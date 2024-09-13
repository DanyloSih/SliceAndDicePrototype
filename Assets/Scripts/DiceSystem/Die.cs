using System;
using UnityEngine;
using Zenject;

namespace SliceAndDicePrototype.DiceSystem
{
    public class Die
    {
        private Side _topSide = Side.Up;
        private Sides<DieSide> _dieSides;

        public Sides<DieSide> DieSides { get => _dieSides; }
        public Side TopSide { get => _topSide; set => _topSide = value; }

        public Die(Side activeSide, Sides<DieSide> dieSides)
        {
            _topSide = activeSide;
            _dieSides = dieSides;
        }
    }
}
