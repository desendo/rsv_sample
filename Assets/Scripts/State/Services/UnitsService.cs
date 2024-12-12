using System.Collections.Generic;
using Game.State.Data;
using Game.State.Data.DataAdapters;
using Game.State.Models;
using UnityEngine;

namespace Game.Services
{
    public class UnitsService : ISaveData<StateData>, ILoadData<StateData>, IModelList<ISelectableModel>,
        IModelList<IModel>, IModelList<IWorldModel>
    {
        private readonly List<IModel> _models = new();
        private readonly List<ISelectableModel> _selectableModels = new();
        private readonly List<IWorldModel> _worldModels = new();
        private readonly StorageAdapter _storageAdapter;
        List<IModel> IModelList<IModel>.GetList() => _models;

        List<ISelectableModel> IModelList<ISelectableModel>.GetList() => _selectableModels;
        List<IWorldModel> IModelList<IWorldModel>.GetList() => _worldModels;

        public UnitsService()
        {
            _models.Add(Hero);
            _selectableModels.Add(Hero);
            _worldModels.Add(Hero);

            _storageAdapter = new StorageAdapter();
        }

        public HeroModel Hero { get; } = new();

        public StorageModel HeroStorage { get; } = new();
        public HeroParameters HeroParameters { get; } = new();

        public void LoadFrom(in StateData data)
        {
            Hero.Position.Value = data.HeroData.Position;
            Hero.Rotation.Value = Quaternion.Euler(0, data.HeroData.Rotation, 0);
            Hero.InventoryShown.Value = data.HeroData.InventoryShown;
            Hero.Speech.Value = data.HeroData.Speech;
            Hero.HasWayPoint.Value = data.HeroData.HasWayPoint;
            Hero.WayPoint.Value = data.HeroData.WayPoint;
            Hero.Selected.Value = data.HeroData.Selected;

            if (data.HeroData.CurrentJob == null)
                Hero.CurrentJob.Value = null;
            else
                Hero.CurrentJob.Value = new HeroModel.Job
                {
                    JobTargetUid = data.HeroData.CurrentJob.JobTargetUid,
                };

            _storageAdapter.DataToModel(data.HeroStorageItemsData, HeroStorage);


            HeroParameters.MassParameter.SetMax(data.HeroParametersData.MaxMass);
            HeroParameters.HungerParameter.SetCurrent(data.HeroParametersData.Hunger);
            HeroParameters.HungerParameter.SetMax(data.HeroParametersData.HungerMax);
            HeroParameters.MaxMoveSpeed.Value = data.HeroParametersData.MaxMoveSpeed;
            HeroParameters.MoveSpeedFactor.Value = data.HeroParametersData.MoveSpeedFactor;
        }


        public void SaveTo(StateData data)
        {
            data.HeroData.Selected = Hero.Selected.Value;
            data.HeroData.Position = Hero.Position.Value;
            data.HeroData.Rotation = Hero.Rotation.Value.eulerAngles.y;
            data.HeroData.Speech = Hero.Speech.Value;
            data.HeroData.HasWayPoint = Hero.HasWayPoint.Value;
            data.HeroData.WayPoint = Hero.WayPoint.Value;
            data.HeroData.InventoryShown = Hero.InventoryShown.Value;

            if (Hero.CurrentJob.Value == null)
                data.HeroData.CurrentJob = null;
            else
                data.HeroData.CurrentJob = new JobData
                {
                    JobTargetUid = Hero.CurrentJob.Value.JobTargetUid,
                };


            _storageAdapter.ModelToData(HeroStorage, data.HeroStorageItemsData);
            data.HeroParametersData.MaxMass = HeroParameters.MassParameter.Max.Value;
            data.HeroParametersData.Hunger = HeroParameters.HungerParameter.Current.Value;
            data.HeroParametersData.HungerMax = HeroParameters.HungerParameter.Max.Value;
            data.HeroParametersData.MaxMoveSpeed = HeroParameters.MaxMoveSpeed.Value;
            data.HeroParametersData.MoveSpeedFactor = HeroParameters.MoveSpeedFactor.Value;
        }
    }
}