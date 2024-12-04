using System;
using System.Collections.Generic;
using Game.Signals;
using Game.State.Models;
using Modules.Common;
using UnityEngine;

namespace Game.Views.UI
{
    public class BagItemView : MonoBehaviour, IDisposable
    {
        private readonly List<IDisposable> _subscriptions = new();

        public int UId { get; private set; }

        public void Dispose()
        {
            _subscriptions?.DisposeAndClear();
        }

        public void Bind(ItemModel model)
        {
            UId = model.UId;
            transform.localPosition = model.ViewPosition.Value;
            transform.localRotation = Quaternion.Euler(0, 0, model.ViewRotation.Value);
            model.PreSave.Subscribe(HandlePreSave).AddTo(_subscriptions);
        }

        private void HandlePreSave()
        {
            Di.Instance.Get<SignalBus>().Fire(
                new UIViewSignals.ActualizeBagItemPositionRequest(UId, transform.localPosition,
                    transform.localRotation));
        }
    }
}