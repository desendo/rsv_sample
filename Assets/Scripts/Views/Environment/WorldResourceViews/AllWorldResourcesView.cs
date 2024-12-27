using System.Collections.Generic;
using System.Linq;
using Game.Services;
using Game.State.Models;
using UnityEngine;

namespace Game.Views.Environment.ResourceViews
{
    public class AllWorldResourcesView : MonoBehaviour
    {
        //todo pools
        private readonly List<WorldResourceView> _views = new();

        private GameConfig _gameConfig;
        private WorldResourcesService _worldResourcesService;

        private void Awake()
        {
            _gameConfig = Di.Instance.Get<GameConfig>();
            _worldResourcesService = Di.Instance.Get<WorldResourcesService>();

            HandleNewResourcesList(_worldResourcesService);
            _worldResourcesService.OnNew.Subscribe(HandleNewResourcesList);
            _worldResourcesService.OnClear.Subscribe(Clear);
            _worldResourcesService.OnAdd.Subscribe(HandleNewResourceModelAdd);
            _worldResourcesService.OnRemove.Subscribe(HandleNewResourceModelRemove);
        }

        private void HandleNewResourceModelRemove(WorldResourceModel obj)
        {
            foreach (var singleResourceView in _views)
                if (singleResourceView.BoundToModel(obj))
                {
                    singleResourceView.Dispose();
                    Destroy(singleResourceView.gameObject);
                }
        }

        private void HandleNewResourceModelAdd(WorldResourceModel obj)
        {
            AddSingleView(obj);
        }

        private void HandleNewResourcesList(IEnumerable<WorldResourceModel> obj)
        {
            Clear();
            foreach (var resourceModel in obj) AddSingleView(resourceModel);
        }

        private void AddSingleView(WorldResourceModel worldResourceModel)
        {
            //здесь мы используем линк, но можно (и нужно) сделать кеширование по айдишникам в GameConfig
            var prefab = _gameConfig.ResourcePrefabEntries.FirstOrDefault(x => x.Id == worldResourceModel.TypeId.Value)
                ?.Prefab;
            if (prefab == null)
            {
                Debug.LogError($"missing prefab for id {worldResourceModel.TypeId.Value}");
                return;
            }

            var instance = Instantiate(prefab, transform);
            _views.Add(instance);
            instance.Bind(worldResourceModel);
        }

        private void Clear()
        {
            foreach (var singleResourceView in _views)
            {
                singleResourceView.Dispose();
                Destroy(singleResourceView.gameObject);
            }

            _views.Clear();
        }
    }
}