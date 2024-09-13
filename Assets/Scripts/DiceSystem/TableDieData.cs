using DG.Tweening;
using UnityEngine;

namespace SliceAndDicePrototype.DiceSystem
{
    public struct TableDieData
    {
        public readonly DieView DieView;
        public readonly Sides<DieSideData> DieSidesData;
        public Side Side;
        public Vector3 PositionOnTable;
        public Tween CurrentTween;

        public TableDieData(DieView dieView, Side side, Sides<DieSideData> dieSidesData, Vector3 positionOnTable, Tween currentTween)
        {
            DieView = dieView;
            PositionOnTable = positionOnTable;
            CurrentTween = currentTween;
            Side = side;
            DieSidesData = dieSidesData;
        }
    }
}
