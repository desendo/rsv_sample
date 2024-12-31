using System.Collections.Generic;
using Game.Services.ServicesBase;
using Game.State.Data;
using Game.State.Models;
using UnityEngine;

namespace Game.Services
{
    public class WorldItemsService : InventoryServiceBase<WorldItemModel>, ISaveLoadData<StateData>,
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
            foreach (var itemData in data.WorldItemsData)
            {
                var model = new WorldItemModel();
                model.TypeId.Value = itemData.TypeId;
                model.Position.Value = itemData.Position;
                model.Rotation.Value = Quaternion.Euler(0, itemData.Rotation, 0);
                model.Selected.Value = itemData.Selected;
                model.UId = itemData.UId == 0 ? StateData.GenerateUid() : itemData.UId;
                Add(model);
            }
        }

        public void SaveTo(StateData data)
        {
            data.WorldItemsData.Clear();
            foreach (var model in this)
                data.WorldItemsData.Add(new WorldItemData
                {
                    Position = model.Position.Value,
                    Rotation = model.Rotation.Value.eulerAngles.y,
                    TypeId = model.TypeId.Value,
                    Selected = model.Selected.Value,
                    UId = model.UId
                });
        }
    }
}