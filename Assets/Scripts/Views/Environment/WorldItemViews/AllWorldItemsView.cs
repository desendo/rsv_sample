using System.Collections.Generic;
using System.Linq;
using Game.Services;
using Game.State.Models;
using Game.Views.Environment.Items;
using UnityEngine;

namespace Game.Views.Environment.Items
{
    public class AllWorldItemsView : MonoBehaviour
    {
        //todo pools
        private readonly List<WorldItemView> _views = new();

        private GameConfig _gameConfig;
        private WorldItemsService _worldItemsService;

        private void Awake()
        {
            _gameConfig = Di.Instance.Get<GameConfig>();
            _worldItemsService = Di.Instance.Get<WorldItemsService>();

            HandleNewWorldItemModelsList(_worldItemsService.WorldItemModels.ToList());

            _worldItemsService.WorldItemModels.OnNew.Subscribe(HandleNewWorldItemModelsList);
            _worldItemsService.WorldItemModels.OnClear.Subscribe(ClearWorldItemModelList);
            _worldItemsService.WorldItemModels.OnAdd.Subscribe(HandleWorldItemModelAdd);
            _worldItemsService.WorldItemModels.OnRemove.Subscribe(HandleWorldItemModelRemove);
        }

        private void HandleWorldItemModelRemove(WorldItemModel obj)
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

        private void HandleWorldItemModelAdd(WorldItemModel model)
        {
            AddSingleView(model);
        }

        private void HandleNewWorldItemModelsList(List<WorldItemModel> obj)
        {
            ClearWorldItemModelList();
            foreach (var resourceModel in obj) AddSingleView(resourceModel);
        }

        private void AddSingleView(WorldItemModel model)
        {
            PrefabEntry<WorldItemView> prefabEntry = null;
            foreach (var x in _gameConfig.WorldItemsPrefabEntries)
            {
                if (x.Id == model.TypeId.Value)
                {
                    prefabEntry = x;
                    break;
                }
            }

            var prefab = prefabEntry?.Prefab;
            if (prefab == null)
            {
                Debug.LogError($"missing prefab for id {model.TypeId.Value}");
                return;
            }

            var instance = Instantiate(prefab, transform);
            _views.Add(instance);
            instance.Bind(model);
        }

        private void ClearWorldItemModelList()
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