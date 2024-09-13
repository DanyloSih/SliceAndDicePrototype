using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SliceAndDicePrototype.DiceSystem
{
    public class RandomPositionsPool
    {
        private HashSet<Vector3> _availablePositions;

        public RandomPositionsPool(List<Transform> points)
        {
            _availablePositions = new HashSet<Vector3>();
            foreach (Transform t in points)
            {
                _availablePositions.Add(t.position);
            }
        }

        public Vector3 GetRandomPosition()
        {
            if (_availablePositions.Count == 0)
            {
                throw new InvalidOperationException("Positions pool is empty!");
            }

            int pointId = UnityEngine.Random.Range(0, _availablePositions.Count);
            Vector3 position = _availablePositions.ElementAt(pointId);
            _availablePositions.Remove(position);
            return position;
        }

        public void ReturnPosition(Vector3 position)
        {
            _availablePositions.Add(position);
        }
    }
}
