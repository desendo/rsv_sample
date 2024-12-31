using System.Collections.Generic;
using Game.Services;
using Game.State.Data;
using Game.State.Models;
using UnityEngine;

namespace Game.Rules
{
    public class DoHeroJobRule
    {
        private readonly DialogsService _dialogsService;
        private readonly GameConfig _gameConfig;
        private readonly HeroService _heroService;
        private readonly List<IModelEnum<IModel>> _modelServices;
        private readonly WorldItemsService _worldItemsService;

        public DoHeroJobRule(HeroService heroService, GameConfig gameConfig,
            WorldItemsService worldItemsService,
            DialogsService dialogsService,
            List<IModelEnum<IModel>> modelServices)
        {
            _heroService = heroService;
            _gameConfig = gameConfig;
            _worldItemsService = worldItemsService;
            _dialogsService = dialogsService;
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
            if (job.JobTargetUid == 0)
                return;

            IModel targetModel = null;
            foreach (var modelService in _modelServices)
            foreach (var model in modelService.GetEnum())
                if (model.UId == job.JobTargetUid)
                    targetModel = model;

            if (targetModel == null)
            {
                Debug.LogError("no Model for id " + job.JobTargetUid);
                _heroService.Hero.CurrentJob.Value = null;
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
                    item.ViewPosition.Value = new Vector2(Random.value * 0.1f, 0);

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
                _worldItemsService.Remove(itemModel);
                var itemParam = _gameConfig.GetItemParam(itemModel.TypeId.Value);

                var item = new StorageItemModel
                {
                    TypeId = { Value = itemModel.TypeId.Value },
                    UId = StateData.GenerateUid(),
                    Mass = { Value = itemParam.Mass },
                    Scale = { Value = itemParam.Size },
                    ViewPosition = { Value = new Vector2(Random.value * 0.1f, 0) }
                };
                _heroService.Hero.Say($"Поднял {itemNameTitle}");
                _heroService.HeroStorage.Items.Add(item);

                _heroService.Hero.OnAction.Invoke();
            }

            if (targetModel is NpcModel npcModel)
                if (!string.IsNullOrEmpty(npcModel.DialogConfigId))
                {
                    _dialogsService.StartDialogRequest.Invoke(npcModel.DialogConfigId, npcModel.DialogUId);
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