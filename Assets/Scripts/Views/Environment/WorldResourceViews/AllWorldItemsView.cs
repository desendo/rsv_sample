using System.Collections.Generic;
using System.Linq;
using Game.Services;
using Game.State.Models;
using Game.Views.Environment.Items;
using UnityEngine;

namespace Game.Views.Environment.ResourceViews
{
    public class AllWorldItemsView : MonoBehaviour
    {
        private readonly List<WorldItemView>
            _views = new(); //сделано без пула. но если делать пул то он должен быть в этом классе

        private GameConfig _gameConfig;
        private WorldItemsService _worldItemsService;

        private void Awake()
        {
            _gameConfig = Di.Instance.Get<GameConfig>();
            _worldItemsService = Di.Instance.Get<WorldItemsService>();

            HandleNewResourcesList(_worldItemsService.WorldItemModels.ToList());
            _worldItemsService.WorldItemModels.OnNew.Subscribe(HandleNewResourcesList);
            _worldItemsService.WorldItemModels.OnClear.Subscribe(Clear);
            _worldItemsService.WorldItemModels.OnAdd.Subscribe(HandleNewResourceModelAdd);
            _worldItemsService.WorldItemModels.OnRemove.Subscribe(HandleNewResourceModelRemove);
        }

        private void HandleNewResourceModelRemove(WorldItemModel obj)
        {
            var count = _views.Count;
            for (var index = 0; index < count; index++)
            {
                var singleResourceView = _views[index];
                if (singleResourceView.BoundToModel(obj))
                {
                    singleResourceView.Dispose();
                    count--;
                    _views.RemoveAt(index);
                    Destroy(singleResourceView.gameObject);
                }
            }
        }

        private void HandleNewResourceModelAdd(WorldItemModel obj)
        {
            AddSingleView(obj);
        }

        private void HandleNewResourcesList(List<WorldItemModel> obj)
        {
            Clear();
            foreach (var resourceModel in obj) AddSingleView(resourceModel);
        }

        private void AddSingleView(WorldItemModel model)
        {
            //здесь мы используем линк, но можно (и нужно) сделать кеширование по айдишникам в GameConfig
            var prefab = _gameConfig.WorldItemsPrefabEntries.FirstOrDefault(x => x.Id == model.TypeId.Value)
                ?.Prefab;
            if (prefab == null)
            {
                Debug.LogError($"missing prefab for id {model.TypeId.Value}");
                return;
            }

            var instance = Instantiate(prefab, transform);
            _views.Add(instance);
            instance.Bind(model);
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