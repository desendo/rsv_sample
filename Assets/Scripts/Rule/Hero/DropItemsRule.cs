using Game.Services;
using Game.Signals;
using Game.State.Data;
using Game.State.Models;
using Modules.Common;
using UnityEngine;

namespace Game.Rules
{
    public class DropItemsRule
    {
        private readonly UnitsService _unitsService;
        private readonly HintService _hintService;
        private readonly WorldItemsService _worldItemsService;
        private readonly GameConfig _gameConfig;

        public DropItemsRule(ISignalBus signalBus, UnitsService unitsService, HintService hintService,
            WorldItemsService worldItemsService, GameConfig gameConfig)
        {
            _unitsService = unitsService;
            _hintService = hintService;
            _worldItemsService = worldItemsService;
            _gameConfig = gameConfig;
            signalBus.Subscribe<UIViewSignals.DropItemHeroStorageRequest>(HandleDropItemHeroStorageRequest);
        }

        private void HandleDropItemHeroStorageRequest(UIViewSignals.DropItemHeroStorageRequest obj)
        {
            StorageItemModel model = null;
            foreach (var x in _unitsService.HeroStorage.Items)
            {
                if (x.UId == obj.Uid)
                {
                    model = x;
                    break;
                }
            }

            if (model != null)
            {
                var randomForRotation = Random.value * 360f;
                var randomForPosition = Random.insideUnitCircle * 0.3f;

                _worldItemsService.WorldItemModels.Add(new WorldItemModel()
                {
                    UId = StateData.GenerateUid(),
                    TypeId = { Value = model.TypeId.Value},
                    Position = { Value = _unitsService.Hero.Position.Value + new Vector3(randomForPosition.x,0, randomForPosition.y)},
                    Rotation = { Value = Quaternion.Euler(0, randomForRotation , 0)}
                });
                _unitsService.Hero.Say($"Упало: {_gameConfig.Localization.GetObjectTitle(model.TypeId.Value)}");
                _unitsService.HeroStorage.Items.Remove(model);
                _hintService.Reset();

            }
        }
    }
}