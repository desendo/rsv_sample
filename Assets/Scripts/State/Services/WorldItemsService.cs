using System.Collections.Generic;
using Game.State.Data;
using Game.State.Models;
using Modules.Reactive.Values;
using UnityEngine;

namespace Game.Services
{
    public class WorldItemsService : ISaveData<StateData>, ILoadData<StateData>, IModelList<ISelectableModel>,
        IModelList<IModel>
    {
        private readonly List<IModel> _models = new();
        private readonly List<ISelectableModel> _selectableModels = new();
        List<IModel> IModelList<IModel>.GetList() => _models;

        List<ISelectableModel> IModelList<ISelectableModel>.GetList() => _selectableModels;
        public IReactiveCollection<WorldItemModel> WorldItemModels { get; } = new ReactiveCollection<WorldItemModel>();

        public WorldItemsService()
        {
            WorldItemModels.Subscribe(OnCollectionChange);
        }

        public void LoadFrom(in StateData data)
        {
            WorldItemModels.Clear();
            foreach (var itemData in data.WorldItemsData)
            {
                var model = new WorldItemModel();
                model.TypeId.Value = itemData.TypeId;
                model.Position.Value = itemData.Position;
                model.Rotation.Value = Quaternion.Euler(0, itemData.Rotation, 0);
                model.Selected.Value = itemData.Selected;
                model.UId = itemData.UId == 0 ? StateData.GenerateUid() : itemData.UId;
                WorldItemModels.Add(model);
            }
        }

        public void SaveTo(StateData data)
        {
            data.WorldItemsData.Clear();
            foreach (var model in WorldItemModels)
                data.WorldItemsData.Add(new WorldItemData()
                {
                    Position = model.Position.Value,
                    Rotation = model.Rotation.Value.eulerAngles.y,
                    TypeId = model.TypeId.Value,
                    Selected = model.Selected.Value,
                    UId = model.UId
                });
        }

        private void OnCollectionChange(List<WorldItemModel> result, IReactiveCollection<WorldItemModel>.EventType eventType)
        {
            if (eventType == IReactiveCollection<WorldItemModel>.EventType.Add)
            {
                foreach (var model in result)
                {
                    _selectableModels.Add(model);
                    _models.Add(model);
                }
            }
            if (eventType == IReactiveCollection<WorldItemModel>.EventType.Remove)
            {
                foreach (var model in result)
                {
                    _selectableModels.Remove(model);
                    _models.Remove(model);
                }
            }
            if (eventType == IReactiveCollection<WorldItemModel>.EventType.Clear)
            {
                _selectableModels.Clear();
                _models.Clear();
            }
            if (eventType == IReactiveCollection<WorldItemModel>.EventType.New)
            {
                _selectableModels.Clear();
                _models.Clear();
                foreach (var model in result)
                {
                    _selectableModels.Add(model);
                    _models.Add(model);
                }
            }
        }
    }
}