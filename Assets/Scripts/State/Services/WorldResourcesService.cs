using System.Collections.Generic;
using Game.State.Data;
using Game.State.Models;
using Modules.Reactive.Values;
using UnityEngine;

namespace Game.Services
{
    public class WorldResourcesService : ISaveData<StateData>, ILoadData<StateData>, IModelList<ISelectableModel>,
        IModelList<IModel>
    {
        private readonly List<IModel> _models = new();
        private readonly List<ISelectableModel> _selectableModels = new();
        List<IModel> IModelList<IModel>.GetList() => _models;

        List<ISelectableModel> IModelList<ISelectableModel>.GetList() => _selectableModels;
        public IReactiveCollection<WorldResourceModel> WorldResourceModels { get; } = new ReactiveCollection<WorldResourceModel>();
        public WorldResourcesService()
        {
            WorldResourceModels.Subscribe(OnCollectionChange);
        }
        public void LoadFrom(in StateData data)
        {
            WorldResourceModels.Clear();
            foreach (var itemData in data.WorldResourcesData)
            {
                var model = new WorldResourceModel();
                model.TypeId.Value = itemData.TypeId;
                model.Position.Value = itemData.Position;
                model.Rotation.Value = Quaternion.Euler(0, itemData.Rotation, 0);
                model.Count.Value = itemData.Count;
                model.Selected.Value = itemData.Selected;
                model.Capacity.Value = itemData.Capacity;
                model.ItemType = itemData.ItemType;
                model.UId = itemData.UId == 0 ? StateData.GenerateUid() : itemData.UId;
                WorldResourceModels.Add(model);
            }
        }

        public void SaveTo(StateData data)
        {
            data.WorldResourcesData.Clear();
            foreach (var model in WorldResourceModels)
                data.WorldResourcesData.Add(new WorldResourceData
                {
                    Position = model.Position.Value,
                    Rotation = model.Rotation.Value.eulerAngles.y,
                    TypeId = model.TypeId.Value,
                    Count = model.Count.Value,
                    Capacity = model.Capacity.Value,
                    Selected = model.Selected.Value,
                    ItemType = model.ItemType,
                    UId = model.UId
                });
        }

        private void OnCollectionChange(List<WorldResourceModel> result, IReactiveCollection<WorldResourceModel>.EventType eventType)
        {
            if (eventType == IReactiveCollection<WorldResourceModel>.EventType.Add)
            {
                foreach (var model in result)
                {
                    _selectableModels.Add(model);
                    _models.Add(model);
                }
            }
            if (eventType == IReactiveCollection<WorldResourceModel>.EventType.Remove)
            {
                foreach (var model in result)
                {
                    _selectableModels.Remove(model);
                    _models.Remove(model);
                }
            }
            if (eventType == IReactiveCollection<WorldResourceModel>.EventType.Clear)
            {
                _selectableModels.Clear();
                _models.Clear();
            }
            if (eventType == IReactiveCollection<WorldResourceModel>.EventType.New)
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