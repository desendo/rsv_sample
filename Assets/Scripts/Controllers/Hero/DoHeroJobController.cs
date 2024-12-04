using System.Collections.Generic;
using Game.Services;
using Game.State.Enum;
using Game.State.Models;
using Modules.Common;
using UnityEngine;

namespace Game.Controllers
{
    public class DoHeroJobController
    {
        private readonly GameConfig _gameConfig;
        private readonly WorldItemsService _worldItemsService;
        private readonly HeroService _heroService;
        private readonly List<IModelList<IModel>> _modelServices;

        public DoHeroJobController(ISignalBus signalBus, HeroService heroService, GameConfig gameConfig,
            WorldItemsService worldItemsService,
            List<IModelList<IModel>> modelServices)
        {
            _heroService = heroService;
            _gameConfig = gameConfig;
            _worldItemsService = worldItemsService;
            _modelServices = modelServices;
            _heroService.Hero.HasWayPoint.Subscribe(OnWayPointDisappear);
            _heroService.Hero.CurrentJob.Subscribe(OnNewJobAppeared);
        }

        private void OnNewJobAppeared(HeroModel.Job obj)
        {
            if (_heroService.Hero.HasWayPoint.Value)
                return;
            if (obj == null)
                return;

            PerformJob(obj);
        }

        private void PerformJob(HeroModel.Job job)
        {
            IModel targetModel = null;
            foreach (var modelService in _modelServices)
            foreach (var model in modelService.GetList())
                if (model.UId == job.JobTargetUid)
                    targetModel = model;

            if (targetModel == null)
            {
                Debug.LogError("no model for id " + job.JobTargetUid);
                _heroService.Hero.CurrentJob.Value = null;
                return;
            }

            if (job.JobId == JobEnum.PickResources)
            {
                if (targetModel is WorldResourceModel resourceModel)
                {
                    var itemNameTitle = _gameConfig.Localization.GetObjectProduct(resourceModel.TypeId.Value);
                    if (resourceModel.Count.Value > 0)
                    {
                        resourceModel.Count.Value--;
                        _heroService.HeroStorage.AddItem(resourceModel.ItemType);

                        _heroService.Hero.Say($"{itemNameTitle} у меня");
                    }
                    else
                    {
                        _heroService.Hero.Say($"{itemNameTitle} тут кончилась");
                    }
                }
                else
                {
                    Debug.LogError("model is not WorldResourceModel " + job.JobTargetUid);
                }
            }
            if (job.JobId == JobEnum.PickItem)
            {
                if (targetModel is WorldItemModel itemModel)
                {
                    var itemNameTitle = _gameConfig.Localization.GetObjectProduct(itemModel.TypeId.Value);
                    _worldItemsService.WorldItemModels.Remove(itemModel);
                    _heroService.HeroStorage.AddItem(itemModel.TypeId.Value);

                    _heroService.Hero.Say($"Поднял {itemNameTitle}");

                }
                else
                {
                    Debug.LogError("model is not WorldResourceModel " + job.JobTargetUid);
                }
            }
            _heroService.Hero.CurrentJob.Value = null;
        }


        private void OnWayPointDisappear(bool hasWayPoint)
        {
            if (hasWayPoint)
                return;
            if (_heroService.Hero.CurrentJob.Value == null)
                return;

            PerformJob(_heroService.Hero.CurrentJob.Value);
        }
    }
}