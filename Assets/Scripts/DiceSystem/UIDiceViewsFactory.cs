using System.Collections.Generic;
using Zenject;

namespace SliceAndDicePrototype.DiceSystem
{
    public class UIDiceViewsFactory
    {
        private UIDieView _uiDieViewPreafab;
        private DiContainer _diContainer;

        public UIDiceViewsFactory(UIDieView uiDieViewPreafab, DiContainer diContainer)
        {
            _uiDieViewPreafab = uiDieViewPreafab;
            _diContainer = diContainer;
        }

        public List<UIDieView> Create(List<Die> dice)
        {
            List<UIDieView> uIDieViews = new List<UIDieView>(dice.Count); 

            foreach (Die d in dice)
            {
                UIDieView uiDieView = _diContainer.InstantiatePrefabForComponent<UIDieView>(_uiDieViewPreafab);
                uiDieView.Initialize(d.DieSides.GetSide(d.TopSide).GetSideData());
                uIDieViews.Add(uiDieView);
            }

            return uIDieViews;
        }
    }
}