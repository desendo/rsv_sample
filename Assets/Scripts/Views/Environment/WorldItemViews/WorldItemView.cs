using System;
using System.Collections.Generic;
using Game.Signals;
using Game.State.Models;
using Modules.Common;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Views.Environment.Items
{
    public class WorldItemView : MonoBehaviour, IDisposable, ISelectableView, IView
    {
        [SerializeField] private GameObject _selectionMarker;
        [SerializeField] private GameObject _hoverMarker;
        private readonly List<IDisposable> _subscriptions = new();
        private WorldItemModel _model;

        private SignalBus _signalBus;

        private void Awake()
        {
            _signalBus = Di.Instance.Get<SignalBus>();
        }

        public void Dispose()
        {
            _subscriptions.DisposeAndClear();
            _model = null;
        }

        //слушаем события на кнопки и отправляем сигналы. здесь передаем ссылку на модель,
        //можно идентификатор как в другом месте. можно и ссылку как тут. но обязательно занулять _model в Dispose
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
                _signalBus.Fire(new WorldViewSignals.SelectRequest(_model));
            if (eventData.button == PointerEventData.InputButton.Right)
                _signalBus.Fire(new WorldViewSignals.ActionRequest(_model));
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _signalBus.Fire(new WorldViewSignals.HoverRequest(_model));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _signalBus.Fire(new WorldViewSignals.UnHoverRequest(_model));
        }

        public void Bind(WorldItemModel model)
        {
            _model = model;
            model.Position.Subscribe(x => transform.position = x).AddTo(_subscriptions);
            model.Rotation.Subscribe(x => transform.rotation = x).AddTo(_subscriptions);
            model.Selected.Subscribe(SetSelected).AddTo(_subscriptions);
            model.Hovered.Subscribe(SetHovered).AddTo(_subscriptions);

        }
        private void SetSelected(bool obj)
        {
            _selectionMarker.gameObject.SetActive(obj);
        }
        private void SetHovered(bool obj)
        {
            _hoverMarker.gameObject.SetActive(obj);
        }

        public bool BoundToModel(WorldItemModel worldItemModel)
        {
            return _model == worldItemModel;
        }
    }
}