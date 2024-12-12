using System;
using System.Collections.Generic;
using Game.Signals;
using Game.State.Models;
using Modules.Common;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Game.Views.UI
{
    public class BagItemView : MonoBehaviour, IDisposable, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject _dropMarker;
        private readonly List<IDisposable> _subscriptions = new();
        private StorageItemModel _model;

        public int UId { get; private set; }

        public void Dispose()
        {
            _subscriptions?.DisposeAndClear();
            _model = null;
            UId = 0;
            _dropMarker.SetActive(false);
        }

        public void Bind(StorageItemModel model)
        {
            _model = model;
            UId = model.UId;
            _dropMarker.SetActive(false);

            transform.localPosition = model.ViewPosition.Value;
            transform.localRotation = Quaternion.Euler(0, 0, model.ViewRotation.Value);
            transform.localScale = Vector3.one * model.Scale.Value;

            model.PreSave.Subscribe(HandlePreSave).AddTo(_subscriptions);
        }

        private void HandlePreSave()
        {
            Di.Instance.Get<SignalBus>().Fire(
                new UIViewSignals.ActualizeBagItemPositionRequest(UId, transform.localPosition,
                    transform.localRotation));
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(eventData.button == PointerEventData.InputButton.Left)
                Di.Instance.Get<SignalBus>().Fire(new UIViewSignals.DropItemHeroStorageRequest(UId));

            if(eventData.button == PointerEventData.InputButton.Right)
                Di.Instance.Get<SignalBus>().Fire(new UIViewSignals.ConsumeItemHeroStorageRequest(UId));
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _dropMarker.SetActive(true);
            Di.Instance.Get<SignalBus>().Fire(new UIViewSignals.HintRequest(_model));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _dropMarker.SetActive(false);
            Di.Instance.Get<SignalBus>().Fire(new UIViewSignals.UnHintRequest(_model));

        }
    }
}