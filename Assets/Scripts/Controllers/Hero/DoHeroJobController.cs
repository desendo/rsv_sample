using System.Collections.Generic;
using Game.Services;
using Game.State.Data;
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
            if(job.JobTargetUid == 0)
                return;

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
            //todo здесь можно добавить стратегию
            if (targetModel is WorldResourceModel resourceModel)
            {
                var itemNameTitle = _gameConfig.Localization.GetObjectProduct(resourceModel.TypeId.Value);
                if (resourceModel.Count.Value > 0)
                {
                    resourceModel.Count.Value--;
                    var item = new StorageItemModel
                        {
                            TypeId = { Value = resourceModel.ItemType },
                            UId = StateData.GenerateUid(),
                            Mass = { Value = _gameConfig.GetItemMass(resourceModel.TypeId.Value)},
                            ViewPosition = { Value = new Vector2(Random.value  * 0.1f, 0)}
                        };

                    _heroService.Hero.Say($"Собрал {itemNameTitle}");
                    _heroService.HeroStorage.Items.Add(item);
                    _heroService.Hero.OnAction.Invoke();

                }
                else
                {
                    _heroService.Hero.Say($"{itemNameTitle} тут кончилась");
                }
            }

            if (targetModel is WorldItemModel itemModel)
            {
                var itemNameTitle = _gameConfig.Localization.GetObjectProduct(itemModel.TypeId.Value);
                _worldItemsService.WorldItemModels.Remove(itemModel);
                var item = new StorageItemModel
                {
                    TypeId = { Value = itemModel.TypeId.Value },
                    UId = StateData.GenerateUid(),
                    Mass = { Value = _gameConfig.GetItemMass(itemModel.TypeId.Value)},
                    ViewPosition = { Value = new Vector2(Random.value  * 0.1f, 0)}
                };
                _heroService.Hero.Say($"Поднял {itemNameTitle}");
                _heroService.HeroStorage.Items.Add(item);

                _heroService.Hero.OnAction.Invoke();
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