using System;
using System.Collections.Generic;
using DG.Tweening;
using Game.Services;
using Game.Signals;
using Game.State.Models;
using Modules.Common;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Views.Hero
{
    public class HeroView : MonoBehaviour, ISelectableView, IView
    {
        [SerializeField] private GameObject _selectionMarker;
        [SerializeField] private GameObject _hoverMarker;
        [SerializeField] private Transform _3d;
        private IModel _model;
        private Sequence _sequence;
        private List<IDisposable> _subscriptions;

        private void Awake()
        {
            //binding to data
            _model = Di.Instance.Get<HeroService>().Hero;
            Di.Instance.Get<HeroService>().Hero.Hovered.Subscribe(SetHovered);
            Di.Instance.Get<HeroService>().Hero.Selected.Subscribe(SetSelected);
            Di.Instance.Get<HeroService>().Hero.Rotation.Subscribe(SetRotation);
            Di.Instance.Get<HeroService>().Hero.Position.Subscribe(SetPosition);
            Di.Instance.Get<HeroService>().Hero.HasWayPoint.Subscribe(HandleHasWayPoint);
        }


        //listen events and emit signals
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
                Di.Instance.Get<ISignalBus>().Fire(new WorldViewSignals.SelectRequest(_model as ISelectableModel));
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Di.Instance.Get<ISignalBus>().Fire(new WorldViewSignals.HoverRequest(_model as ISelectableModel));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Di.Instance.Get<ISignalBus>().Fire(new WorldViewSignals.UnHoverRequest(_model as ISelectableModel));
        }

        private void HandleHasWayPoint(bool hasWayPoint)
        {
            if (hasWayPoint)
                StartWalkAnimation();
            else
                StopWalkAnimation();
        }

        private void StopWalkAnimation()
        {
            _sequence?.Kill();
            _3d.localPosition = Vector3.zero;
            _3d.localRotation = Quaternion.identity;
        }

        private void StartWalkAnimation()
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _3d.localRotation = Quaternion.Euler(0, 0, -1);
            _sequence.Append(_3d.DOLocalMoveY(0.1f, 0.1f).SetEase(Ease.InOutQuad));
            _sequence.Append(_3d.DOLocalMoveY(0, 0.1f).SetEase(Ease.InOutQuad));
            _sequence.Insert(0, _3d.DOLocalRotate(new Vector3(0, 0, 1), 0.2f));
            _sequence.SetLoops(-1, LoopType.Yoyo);
        }

        private void SetPosition(Vector3 obj)
        {
            transform.position = obj;
        }

        private void SetRotation(Quaternion obj)
        {
            transform.rotation = obj;
        }

        private void SetSelected(bool obj)
        {
            _selectionMarker.gameObject.SetActive(obj);
        }

        private void SetHovered(bool obj)
        {
            _hoverMarker.gameObject.SetActive(obj);
        }
    }
}