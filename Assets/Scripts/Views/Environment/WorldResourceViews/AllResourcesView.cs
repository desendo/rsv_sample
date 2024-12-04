using System.Collections.Generic;
using System.Linq;
using Game.Services;
using Game.State.Models;
using UnityEngine;

namespace Game.Views.Environment.ResourceViews
{
    public class AllResourcesView : MonoBehaviour
    {
        private readonly List<WorldResourceView>
            _views = new(); //сделано без пула. но если делать пул то он должен быть в этом классе

        private GameConfig _gameConfig;
        private WorldResourcesService _worldResourcesService;

        private void Awake()
        {
            _gameConfig = Di.Instance.Get<GameConfig>();
            _worldResourcesService = Di.Instance.Get<WorldResourcesService>();

            HandleNewResourcesList(_worldResourcesService.WorldResourceModels.ToList());
            _worldResourcesService.WorldResourceModels.OnNew.Subscribe(HandleNewResourcesList);
            _worldResourcesService.WorldResourceModels.OnClear.Subscribe(Clear);
            _worldResourcesService.WorldResourceModels.OnAdd.Subscribe(HandleNewResourceModelAdd);
            _worldResourcesService.WorldResourceModels.OnRemove.Subscribe(HandleNewResourceModelRemove);
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

        private void HandleNewResourcesList(List<WorldResourceModel> obj)
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