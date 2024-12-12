using System.Collections.Generic;
using Game.Services;
using Game.State.Data;
using Game.State.Models;
using Modules.Common;
using UnityEngine;

namespace Game.Rules
{
    public class DoHeroJobRule
    {
        private readonly GameConfig _gameConfig;
        private readonly WorldItemsService _worldItemsService;
        private readonly UnitsService _unitsService;
        private readonly List<IModelList<IModel>> _modelServices;

        public DoHeroJobRule(ISignalBus signalBus, UnitsService unitsService, GameConfig gameConfig,
            WorldItemsService worldItemsService,
            List<IModelList<IModel>> modelServices)
        {
            _unitsService = unitsService;
            _gameConfig = gameConfig;
            _worldItemsService = worldItemsService;
            _modelServices = modelServices;
            _unitsService.Hero.HasWayPoint.Subscribe(OnWayPointDisappear);
            _unitsService.Hero.CurrentJob.Subscribe(OnNewJobAppeared);
        }

        private void OnNewJobAppeared(HeroModel.Job obj)
        {
            if (_unitsService.Hero.HasWayPoint.Value)
                return;

            if (obj == null)
                return;

            PerformJob(obj);
        }

        private void PerformJob(HeroModel.Job job)
        {
            if(job.JobTargetUid == 0)
                return;

            IModel targetModel = null;
            foreach (var modelService in _modelServices)
            foreach (var model in modelService.GetList())
                if (model.UId == job.JobTargetUid)
                    targetModel = model;

            if (targetModel == null)
            {
                Debug.LogError("no Model for id " + job.JobTargetUid);
                _unitsService.Hero.CurrentJob.Value = null;
                return;
            }
            //todo здесь можно добавить стратегию
            if (targetModel is WorldResourceModel resourceModel)
            {
                var itemNameTitle = _gameConfig.Localization.GetObjectProduct(resourceModel.TypeId.Value);
                if (resourceModel.Resources.Current.Value > 0)
                {
                    resourceModel.Resources.Current.Value--;
                    var item = new StorageItemModel();
                    item.TypeId.Value = resourceModel.ItemType;
                    item.UId = StateData.GenerateUid();

                    var itemParam = _gameConfig.GetItemParam(item.TypeId.Value);
                    item.Mass.Value = itemParam.Mass;
                    item.Scale.Value = itemParam.Size;
                    item.ViewPosition.Value = new Vector2(Random.value  * 0.1f, 0);

                    _unitsService.Hero.Say($"Собрал {itemNameTitle}");
                    _unitsService.HeroStorage.Items.Add(item);
                    _unitsService.Hero.OnAction.Invoke();

                }
                else
                {
                    _unitsService.Hero.Say($"{itemNameTitle} тут кончилась");
                }
            }

            if (targetModel is WorldItemModel itemModel)
            {
                var itemNameTitle = _gameConfig.Localization.GetObjectProduct(itemModel.TypeId.Value);
                _worldItemsService.WorldItemModels.Remove(itemModel);
                var itemParam = _gameConfig.GetItemParam(itemModel.TypeId.Value);

                var item = new StorageItemModel
                {
                    TypeId = { Value = itemModel.TypeId.Value },
                    UId = StateData.GenerateUid(),
                    Mass = { Value = itemParam.Mass},
                    Scale = { Value = itemParam.Size},
                    ViewPosition = { Value = new Vector2(Random.value  * 0.1f, 0)}
                };
                _unitsService.Hero.Say($"Поднял {itemNameTitle}");
                _unitsService.HeroStorage.Items.Add(item);

                _unitsService.Hero.OnAction.Invoke();
            }

            _unitsService.Hero.CurrentJob.Value = null;
        }


        private void OnWayPointDisappear(bool hasWayPoint)
        {
            if (hasWayPoint)
                return;
            if (_unitsService.Hero.CurrentJob.Value == null)
                return;

            PerformJob(_unitsService.Hero.CurrentJob.Value);
        }
    }
}