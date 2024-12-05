using System.Collections.Generic;
using Game.State.Data;
using Game.State.Models;
using UnityEngine;

namespace Game.Services
{
    public class HeroService : ISaveData<StateData>, ILoadData<StateData>, IModelList<ISelectableModel>,
        IModelList<IModel>
    {
        private readonly List<IModel> _models = new();
        private readonly List<ISelectableModel> _selectableModels = new();

        public HeroService()
        {
            _models.Add(Hero);
            _selectableModels.Add(Hero);
        }

        public HeroModel Hero { get; } = new();

        public StorageModel HeroStorage { get; } = new();
        public HeroParameters HeroParameters { get; } = new();

        public void LoadFrom(in StateData data)
        {
            Hero.Position.Value = data.HeroData.Position;
            Hero.Rotation.Value = Quaternion.Euler(0, data.HeroData.Rotation, 0);
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

            HeroStorage.Items.Clear();
            foreach (var dataItem in data.HeroStorageItems)
            {
                var item = new StorageItemModel
                {
                    TypeId = { Value = dataItem.TypeId },
                    UId = dataItem.UId,
                    ViewRotation =
                    {
                        Value = dataItem.ViewRotation
                    },
                    ViewPosition =
                    {
                        Value = dataItem.ViewPosition
                    },
                    Mass = { Value = dataItem.Mass}
                };
                HeroStorage.Items.Add(item);
            }

            HeroParameters.Hunger.Current.Value = data.HeroParametersData.Hunger;
            HeroParameters.Hunger.Max.Value = data.HeroParametersData.HungerMax;
            HeroParameters.MaxMass.Value = data.HeroParametersData.MaxMass;
            HeroParameters.MaxMoveSpeed.Value = data.HeroParametersData.MaxMoveSpeed;
            HeroParameters.MoveSpeedFactor.Value = data.HeroParametersData.MoveSpeedFactor;
        }

        List<IModel> IModelList<IModel>.GetList()
        {
            return _models;
        }

        List<ISelectableModel> IModelList<ISelectableModel>.GetList()
        {
            return _selectableModels;
        }

        public void SaveTo(StateData data)
        {
            data.HeroData.Selected = Hero.Selected.Value;
            data.HeroData.Position = Hero.Position.Value;
            data.HeroData.Rotation = Hero.Rotation.Value.eulerAngles.y;
            data.HeroData.Speech = Hero.Speech.Value;
            data.HeroData.HasWayPoint = Hero.HasWayPoint.Value;
            data.HeroData.WayPoint = Hero.WayPoint.Value;
            data.HeroStorageItems = new List<ItemData>();
            if (Hero.CurrentJob.Value == null)
                data.HeroData.CurrentJob = null;
            else
                data.HeroData.CurrentJob = new JobData
                {
                    JobTargetUid = Hero.CurrentJob.Value.JobTargetUid,
                };

            data.HeroStorageItems.Clear();
            foreach (var item in HeroStorage.Items)
            {
                item.PreSave
                    .Invoke(); //здесь мы эмиттим событие для того чтобы вьюхи успели сообщить нечто важное перед сохранением
                //в данном случае вьюхи содержат физику и (не записывая в модель напрямую) сообщают о своем положении и вращении

                data.HeroStorageItems.Add(new ItemData
                {
                    TypeId = item.TypeId.Value,
                    UId = item.UId,
                    ViewRotation = item.ViewRotation.Value,
                    ViewPosition = item.ViewPosition.Value,
                    Mass = item.Mass.Value,
                });
            }

            data.HeroParametersData.Hunger = HeroParameters.Hunger.Current.Value;
            data.HeroParametersData.HungerMax = HeroParameters.Hunger.Max.Value;
            data.HeroParametersData.MaxMass = HeroParameters.MaxMass.Value;
            data.HeroParametersData.MaxMoveSpeed = HeroParameters.MaxMoveSpeed.Value;
            data.HeroParametersData.MoveSpeedFactor = HeroParameters.MoveSpeedFactor.Value;
        }
    }
}