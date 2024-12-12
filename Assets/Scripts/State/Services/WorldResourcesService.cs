using System.Collections.Generic;
using Game.State.Data;
using Game.State.Models;
using Modules.Reactive.Values;
using UnityEngine;

namespace Game.Services
{
    public class WorldResourcesService : ISaveData<StateData>, ILoadData<StateData>, IModelList<ISelectableModel>,
        IModelList<IModel>, IModelList<IWorldModel>
    {
        private readonly List<IModel> _models = new();
        private readonly List<IWorldModel> _worldModels = new();
        private readonly List<ISelectableModel> _selectableModels = new();
        List<IModel> IModelList<IModel>.GetList() => _models;

        List<IWorldModel> IModelList<IWorldModel>.GetList() => _worldModels;
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
                model.Selected.Value = itemData.Selected;
                model.ItemType = itemData.ItemType;

                model.Resources.DataToModel(itemData.ResourcesData, model.Resources);

                model.UId = itemData.UId == 0 ? StateData.GenerateUid() : itemData.UId;
                WorldResourceModels.Add(model);
            }
        }

        public void SaveTo(StateData data)
        {
            data.WorldResourcesData.Clear();
            foreach (var model in WorldResourceModels)
            {
                var item = new WorldResourceData
                {
                    Position = model.Position.Value,
                    Rotation = model.Rotation.Value.eulerAngles.y,
                    TypeId = model.TypeId.Value,

                    Selected = model.Selected.Value,
                    ItemType = model.ItemType,
                    UId = model.UId
                };
                model.Resources.ModelToData(model.Resources, item.ResourcesData);
                data.WorldResourcesData.Add(item);
            }
        }

        private void OnCollectionChange(List<WorldResourceModel> result, IReactiveCollection<WorldResourceModel>.EventType eventType)
        {
            if (eventType == IReactiveCollection<WorldResourceModel>.EventType.Add)
            {
                foreach (var model in result)
                {
                    _selectableModels.Add(model);
                    _models.Add(model);
                    _worldModels.Add(model);
                }
            }
            if (eventType == IReactiveCollection<WorldResourceModel>.EventType.Remove)
            {
                foreach (var model in result)
                {
                    _selectableModels.Remove(model);
                    _models.Remove(model);
                    _worldModels.Remove(model);
                }
            }
            if (eventType == IReactiveCollection<WorldResourceModel>.EventType.Clear)
            {
                _selectableModels.Clear();
                _models.Clear();
                _worldModels.Clear();
            }
            if (eventType == IReactiveCollection<WorldResourceModel>.EventType.New)
            {
                _selectableModels.Clear();
                _models.Clear();
                _worldModels.Clear();
                foreach (var model in result)
                {
                    _selectableModels.Add(model);
                    _models.Add(model);
                    _worldModels.Add(model);
                }
            }
        }
    }
}