using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using SliceAndDicePrototype.Utilities;
using UnityEngine;

namespace SliceAndDicePrototype.DiceSystem
{
    public class DiceTableView
    {
        [Serializable]
        public struct Settings
        {
            public float MoveToCenterDuration;
            public float RotateToCenterDuration;
            public float MoveToRelatedUIElementDuration;
            public float RotateToRelatedUIElementDuration;
            public float DelayBetweenDieMovesToCenter;
            public float DelayBetweenDieMovesToRelatedUIElement;
        }

        private Settings _settings;
        private RandomPositionsPool _tablePositionsProvider;
        private DieViewFactory _viewFactory;
        private Dictionary<UIDieView, TableDieData> _registeredDiceViews = new Dictionary<UIDieView, TableDieData>();

        public DiceTableView(Settings settings, RandomPositionsPool tablePositionsProvider, DieViewFactory viewFactory)
        {
            _settings = settings;
            _tablePositionsProvider = tablePositionsProvider;
            _viewFactory = viewFactory;
        }

        public Dictionary<UIDieView, Side> GetSides()
        {
            Dictionary<UIDieView, Side> sides = new (_registeredDiceViews.Count);

            foreach (var item in _registeredDiceViews)
            {
                sides.Add(item.Key, item.Value.Side);
            }

            return sides;
        }

        public void RegisterDie(UIDieView relatedUIElement, Side topSide, Sides<DieSideData> dieSidesData)
        {
            DieView dieView = _viewFactory.Create(dieSidesData);
            dieView.gameObject.SetActive(false);
            _registeredDiceViews.Add(relatedUIElement, new TableDieData(dieView, topSide, dieSidesData, Vector3.zero, null));
        }

        public void UnregisterDie(UIDieView relatedUIElement)
        {
            TableDieData tableDieData = _registeredDiceViews[relatedUIElement];
            _registeredDiceViews.Remove(relatedUIElement);
            MonoBehaviour.Destroy(tableDieData.DieView.gameObject);
        }

        public void UnregisterAll()
        {
            foreach (var dieViewKeyValue in _registeredDiceViews)
            {
                MonoBehaviour.Destroy(dieViewKeyValue.Value.DieView.gameObject);
            }

            _registeredDiceViews.Clear();
        }

        public Task MoveDiceViewsToCenter(CancellationToken cancellationToken)
        {
            return InvokeDiceAnimationFunctions(
                MoveDieViewToCenter,
                _settings.DelayBetweenDieMovesToCenter,
                cancellationToken);
        }

        public async Task RollDice(Vector3 rollForce, CancellationToken cancellationToken)
        {
            List<Task> playingTweens = new List<Task>();

            foreach (var dieViewKeyValue in _registeredDiceViews)
            {
                playingTweens.Add(RollDieView(dieViewKeyValue.Key, rollForce, cancellationToken));
            }

            await Task.WhenAll(playingTweens);
            cancellationToken.ThrowIfCancellationRequested();
        }

        public Task MoveDiceViewsToRelatedUIElement(CancellationToken cancellationToken)
        {
            return InvokeDiceAnimationFunctions(
                MoveDieViewToRelatedUIElement, 
                _settings.DelayBetweenDieMovesToRelatedUIElement,
                cancellationToken);
        }

        private Dictionary<UIDieView, TableDieData> GetRegisteredDiceViewsClone()
        {
            return new Dictionary<UIDieView, TableDieData>(_registeredDiceViews);
        }

        private async Task InvokeDiceAnimationFunctions(
            Func<UIDieView, Vector3, Tween> dieAnimationFunc, 
            float delayBetweenDieMoves, 
            CancellationToken cancellationToken)
        {
            List<UniTask> playingTweens = new List<UniTask>();

            var registeredDice = GetRegisteredDiceViewsClone();
            foreach (var dieViewKeyValue in registeredDice)
            {
                var dieTargetRotation = SidesUtilities.SideRotations.GetSide(dieViewKeyValue.Value.Side);
                Tween tween = dieAnimationFunc(dieViewKeyValue.Key, dieTargetRotation.eulerAngles);
                playingTweens.Add(tween.ToUniTask(cancellationToken: cancellationToken));
                await UniTask.WaitForSeconds(delayBetweenDieMoves, cancellationToken: cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
            }

            await UniTask.WhenAll(playingTweens);
            cancellationToken.ThrowIfCancellationRequested();
        }

        private Tween MoveDieViewToCenter(UIDieView relatedUIElement, Vector3 targetRotation)
        {
            TableDieData tableDieData = _registeredDiceViews[relatedUIElement];
            CompleteTween(tableDieData);
            tableDieData.PositionOnTable = _tablePositionsProvider.GetRandomPosition();

            Vector3 uiElementPosition = UIUtilities.GetUIElementWorldPoint(
                relatedUIElement.GetComponent<RectTransform>(), tableDieData.PositionOnTable.y, Camera.main);

            tableDieData.DieView.transform.position = uiElementPosition;
            tableDieData.DieView.gameObject.SetActive(true);
            tableDieData.DieView.DisablePhysics();

            Sequence moveRotateSequence = DOTween.Sequence();
            moveRotateSequence.Append(tableDieData.DieView.transform.DOMove(
                tableDieData.PositionOnTable, _settings.MoveToCenterDuration));

            moveRotateSequence.Join(tableDieData.DieView.transform.DORotate(
                targetRotation, _settings.RotateToCenterDuration));

            moveRotateSequence.Join(relatedUIElement.HideIcon());

            tableDieData.CurrentTween = moveRotateSequence;

            _registeredDiceViews[relatedUIElement] = tableDieData;

            return moveRotateSequence;
        }

        private async Task RollDieView(UIDieView relatedUIElement, Vector3 rollForce, CancellationToken cancellationToken)
        {
            TableDieData tableDieData = _registeredDiceViews[relatedUIElement];
            tableDieData.DieView.EnablePhysics();
            Side side = await tableDieData.DieView.Roll(rollForce, cancellationToken);
            tableDieData.Side = side;
            _registeredDiceViews[relatedUIElement] = tableDieData;
        }

        private Tween MoveDieViewToRelatedUIElement(UIDieView relatedUIElement, Vector3 targetRotation)
        {
            TableDieData tableDieData = _registeredDiceViews[relatedUIElement];
            CompleteTween(tableDieData);
            tableDieData.DieView.DisablePhysics();

            Vector3 uiElementPosition = UIUtilities.GetUIElementWorldPoint(
                relatedUIElement.GetComponent<RectTransform>(), tableDieData.PositionOnTable.y, Camera.main);

            Sequence mainSequence = DOTween.Sequence();

            Sequence moveRotateSequence = DOTween.Sequence();
            moveRotateSequence.Append(tableDieData.DieView.transform.DOMove(
                uiElementPosition, _settings.MoveToCenterDuration));

            moveRotateSequence.Join(tableDieData.DieView.transform.DORotate(
                targetRotation, _settings.RotateToCenterDuration));
            moveRotateSequence.OnComplete(() => {
                _tablePositionsProvider.ReturnPosition(tableDieData.PositionOnTable);
                relatedUIElement.Initialize(tableDieData.DieSidesData.GetSide(tableDieData.Side));
                tableDieData.DieView.gameObject.SetActive(false);
            });

            mainSequence.Append(moveRotateSequence);
            mainSequence.Append(relatedUIElement.ShowIcon());

            tableDieData.CurrentTween = mainSequence;

            _registeredDiceViews[relatedUIElement] = tableDieData;

            return moveRotateSequence;
        }

        private static void CompleteTween(TableDieData tableDieData)
        {
            if (tableDieData.CurrentTween.IsActive())
            {
                tableDieData.CurrentTween.Complete();
                tableDieData.CurrentTween.Kill();
            }
        }
    }
}
