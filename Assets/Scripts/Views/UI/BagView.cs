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
        [SerializeField] private GameObject _panel;
        [SerializeField] private TriggerListener _triggerListener;
        private readonly List<BagItemView> _views = new();
        private GameConfig _gameConfig;
        private StorageModel _heroStorage;
        private SignalBus _signalBus;

        private void Awake()
        {
            _signalBus = Di.Instance.Get<SignalBus>();
            _gameConfig = Di.Instance.Get<GameConfig>();

            Di.Instance.Get<HeroService>().Hero.Selected.Subscribe(selected => _panel.SetActive(selected));

            _heroStorage = Di.Instance.Get<HeroService>().HeroStorage;

            _heroStorage.Items.OnClear.Subscribe(HandleClear);
            _heroStorage.Items.OnAdd.Subscribe(HandleOnAdd);
            _heroStorage.Items.OnRemove.Subscribe(HandleOnRemove);
            _heroStorage.Items.OnNew.Subscribe(FillItems);
            _triggerListener.Other.Subscribe(HandleTrigger);

            FillItems(_heroStorage.Items.ToList());
        }

        private void HandleTrigger(Collider2D other)
        {
            var itemView = other.GetComponent<BagItemView>();
            if (itemView != null) _signalBus.Fire(new UIViewSignals.DropItemHeroStorageRequest(itemView.UId));
        }

        private void HandleOnRemove(ItemModel obj)
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

        private void HandleOnAdd(ItemModel model)
        {
            AddNewView(model);
        }

        private void AddNewView(ItemModel model)
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

        private void FillItems(List<ItemModel> items)
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