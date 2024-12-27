using System.Collections.Generic;
using System.Linq;
using Game.Services;
using Game.Signals;
using Game.State.Models;
using Game.Views.Common;
using Modules.Common;
using UnityEngine;

namespace Game.Views.UI
{
    public class BagView : MonoBehaviour
    {
        [SerializeField] private RectTransform _parent;
        [SerializeField] private RectTransform _scaleHandler;
        [SerializeField] private GameObject _panel;
        [SerializeField] private TriggerListener _triggerListener1;
        [SerializeField] private TriggerListener _triggerListener2;

        [SerializeField] private float _widthOffset;
        [SerializeField] private float _heightOffset;
        [SerializeField] private RectTransform _container;
        private readonly List<BagItemView> _views = new();
        private GameConfig _gameConfig;
        private StorageModel _heroStorage;
        private SignalBus _signalBus;

        private void Awake()
        {
            _signalBus = Di.Instance.Get<SignalBus>();
            _gameConfig = Di.Instance.Get<GameConfig>();

            Di.Instance.Get<HeroService>().Hero.Selected.Subscribe(selected => _panel.SetActive(selected));

            var service = Di.Instance.Get<HeroService>();
            _heroStorage = service.HeroStorage;

            _heroStorage.Items.OnClear.Subscribe(HandleClear);
            _heroStorage.Items.OnAdd.Subscribe(HandleOnAdd);
            _heroStorage.Items.OnRemove.Subscribe(HandleOnRemove);
            _heroStorage.Items.OnNew.Subscribe(FillItems);

            _triggerListener1.Other.Subscribe(HandleTrigger);
            _triggerListener2.Other.Subscribe(HandleTrigger);

            _heroStorage.Width.Subscribe(x => UpdateContainerSize());
            _heroStorage.Height.Subscribe(x => UpdateContainerSize());

            service.Hero.InventoryShown.Subscribe(b => _scaleHandler.localScale = Vector3.one * (b ? _gameConfig.InventoryScale : 1));
            FillItems(_heroStorage.Items);
        }

        private void UpdateContainerSize()
        {
            _container.sizeDelta = new Vector2(_heroStorage.Width.Value + _widthOffset, _heroStorage.Height.Value + _heightOffset);
        }

        private void HandleTrigger(Collider2D other)
        {
            var itemView = other.GetComponent<BagItemView>();
            if (itemView != null) _signalBus.Fire(new UIViewSignals.DropItemHeroStorageRequest(itemView.UId));
        }

        private void HandleOnRemove(StorageItemModel obj)
        {
            var view = _views.FirstOrDefault(x => x.UId == obj.UId);
            if (view == null)
            {
                Debug.LogWarning("cant find item view in bag for " + obj.UId);
                return;
            }

            view.Dispose();
            _views.Remove(view);
            Destroy(view.gameObject);
        }

        private void HandleOnAdd(StorageItemModel model)
        {
            AddNewView(model);
        }

        private void AddNewView(StorageItemModel model)
        {
            var prefab = _gameConfig.BagItemViewPrefabEntries.FirstOrDefault(x => x.Id == model.TypeId.Value)?.Prefab;
            if (prefab == null)
            {
                Debug.LogWarning("cant find bag prefab view  for " + model.TypeId.Value);
                return;
            }

            var viewInstance = Instantiate(prefab, _parent);

            viewInstance.Bind(model);
            _views.Add(viewInstance);
        }

        private void FillItems(ICollection<StorageItemModel> items)
        {
            foreach (var model in items) AddNewView(model);
        }

        private void HandleClear()
        {
            foreach (var view in _views)
            {
                view.Dispose();
                Destroy(view.gameObject);
            }

            _views.Clear();
        }
    }
}