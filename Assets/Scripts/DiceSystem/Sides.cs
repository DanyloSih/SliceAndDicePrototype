using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

namespace SliceAndDicePrototype.DiceSystem
{
    [Serializable]
    public class Sides<T>
    {
        [SerializeField] private T _leftSide;
        [SerializeField] private T _rightSide;
        [SerializeField] private T _upSide;
        [SerializeField] private T _downSide;
        [SerializeField] private T _forwardSide;
        [SerializeField] private T _backSide;

        public Sides(T leftSide, T rightSide, T upSide, T downSide, T forwardSide, T backSide)
        {
            _leftSide = leftSide;
            _rightSide = rightSide;
            _upSide = upSide;
            _downSide = downSide;
            _forwardSide = forwardSide;
            _backSide = backSide;
        }

        public T LeftSide { get => _leftSide; }
        public T RightSide { get => _rightSide; }
        public T UpSide { get => _upSide; }
        public T DownSide { get => _downSide; }
        public T ForwardSide { get => _forwardSide; }
        public T BackSide { get => _backSide; }

        public T GetSide(Side side)
        {
            switch (side)
            {
                case Side.Left:
                    return _leftSide;
                case Side.Right:
                    return _rightSide;
                case Side.Up:
                    return _upSide;
                case Side.Down:
                    return _downSide;
                case Side.Forward:
                    return _forwardSide;
                case Side.Back:
                    return _backSide;
                default:
                    throw new NotImplementedException();
            }
        }

        public IEnumerable<SideWithData<T>> GetSides()
        {
            yield return new SideWithData<T>(Side.Left, _leftSide);
            yield return new SideWithData<T>(Side.Right, _rightSide);
            yield return new SideWithData<T>(Side.Up, _upSide);
            yield return new SideWithData<T>(Side.Down, _downSide);
            yield return new SideWithData<T>(Side.Forward, _forwardSide);
            yield return new SideWithData<T>(Side.Back,  _backSide);
        }

        public T[] CreateSidesArray()
        {
            return new T[] { _leftSide, _rightSide, _upSide, _downSide, _forwardSide, _backSide };
        }

        public Sides<TNew> ConvertToNewSidesType<TNew>(Func<T, Side, TNew> convertFunc)
        {
            return new Sides<TNew>(
                convertFunc(GetSide(Side.Left), Side.Left),
                convertFunc(GetSide(Side.Right), Side.Right),
                convertFunc(GetSide(Side.Up), Side.Up),
                convertFunc(GetSide(Side.Down), Side.Down),
                convertFunc(GetSide(Side.Forward), Side.Forward),
                convertFunc(GetSide(Side.Back), Side.Back)
                );
        }
    }
}
