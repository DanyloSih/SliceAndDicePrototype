using System;
using UnityEngine;
using Zenject;

namespace SliceAndDicePrototype.DiceSystem
{
    public class DieViewFactory
    {
        [Serializable]
        public struct Settings
        {
           public DieView DieViewPrefab;
           public Transform NewInstancesParent;
        }

        private DiContainer _container;
        private Settings _settings;

        public DieViewFactory(
            Settings settings,
            DiContainer container)
        {
            _settings = settings;
            _container = container;
        }

        public DieView Create(Sides<DieSideData> dieSidesData)
        {
            DieView dieView = _container.InstantiatePrefabForComponent<DieView>(_settings.DieViewPrefab);
            dieView.Initialize(dieSidesData);
            dieView.transform.parent = _settings.NewInstancesParent;
            return dieView;
        }
    }
}
