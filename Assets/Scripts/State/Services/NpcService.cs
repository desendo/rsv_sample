using System.Collections.Generic;
using Game.Services.ServicesBase;
using Game.State.Data;
using Game.State.Models;
using UnityEngine;

namespace Game.Services
{
    public class NpcService : InventoryServiceBase<NpcModel>, ISaveLoadData<StateData>,
        IModelEnum<ISelectableModel>, IModelEnum<IModel>, IModelEnum<IWorldModel>, IModelEnum<IInteractionModel>
    {
        IEnumerable<IModel> IModelEnum<IModel>.GetEnum() => GetByInterface<IModel>();
        IEnumerable<IInteractionModel> IModelEnum<IInteractionModel>.GetEnum() => GetByInterface<IInteractionModel>();
        IEnumerable<IWorldModel> IModelEnum<IWorldModel>.GetEnum() => GetByInterface<IWorldModel>();
        IEnumerable<ISelectableModel> IModelEnum<ISelectableModel>.GetEnum() => GetByInterface<ISelectableModel>();

        private HashSet<int> _removedModels = new HashSet<int>();
        public NpcModel TryCreateOrGetModelFromView(NpcModelData data, MonoBehaviour view)
        {
            foreach (var model in this)
            {
                if (model.UId == data.UId)
                    return model;
            }

            if (_removedModels.Contains(data.UId))
                return null;

            var uid = data.UId;
            var newModel = new NpcModel();
            newModel.UId = uid;
            newModel.DialogConfigId = data.DialogConfigId;
            newModel.DialogUId = data.DialogUId;
            newModel.Position.Value = view.transform.position;
            newModel.Rotation.Value = view.transform.rotation;
            Add(newModel);
            return newModel;
        }


        public void SaveTo(StateData data)
        {

        }

        public void LoadFrom(in StateData data)
        {
            
        }


    }
}