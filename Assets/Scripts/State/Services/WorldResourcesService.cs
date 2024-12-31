using System.Collections.Generic;
using Game.Services.ServicesBase;
using Game.State.Data;
using Game.State.Models;
using UnityEngine;

namespace Game.Services
{
    public class WorldResourcesService : InventoryServiceBase<WorldResourceModel>, ISaveLoadData<StateData>,
        IModelEnum<ISelectableModel>, IModelEnum<IModel>, IModelEnum<IWorldModel>, IModelEnum<IInteractionModel>
    {
        IEnumerable<IInteractionModel> IModelEnum<IInteractionModel>.GetEnum()
        {
            return GetByInterface<IInteractionModel>();
        }

        IEnumerable<IModel> IModelEnum<IModel>.GetEnum()
        {
            return GetByInterface<IModel>();
        }

        IEnumerable<ISelectableModel> IModelEnum<ISelectableModel>.GetEnum()
        {
            return GetByInterface<ISelectableModel>();
        }

        IEnumerable<IWorldModel> IModelEnum<IWorldModel>.GetEnum()
        {
            return GetByInterface<IWorldModel>();
        }

        public void LoadFrom(in StateData data)
        {
            Clear();
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
                Add(model);
            }
        }

        public void SaveTo(StateData data)
        {
            data.WorldResourcesData.Clear();
            foreach (var model in this)
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
    }
}