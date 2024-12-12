﻿using Game.Services;
using Game.Signals;
using Game.State.Models;
using Modules.Common;

namespace Game.Rules
{
    public class ConsumeItemsRule
    {
        private readonly UnitsService _unitsService;
        private readonly GameConfig _gameConfig;

        public ConsumeItemsRule(ISignalBus signalBus, UnitsService unitsService, GameConfig gameConfig)
        {
            _unitsService = unitsService;
            _gameConfig = gameConfig;
            signalBus.Subscribe<UIViewSignals.ConsumeItemHeroStorageRequest>(HandleConsumeItemHeroStorageRequest);
        }

        private void HandleConsumeItemHeroStorageRequest(UIViewSignals.ConsumeItemHeroStorageRequest obj)
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

            if (model == null) return;

            foreach (var entry in _gameConfig.ItemActionsResult)
            {
                if (model.TypeId.Value != entry.Id) continue;

                foreach (var result in entry.Value)
                {
                    if (result.ActionType != ActionType.Consume) continue;

                    _unitsService.Hero.Say("Использовал "+_gameConfig.Localization.GetObjectTitle(model.TypeId.Value));
                    foreach (var effect in result.InstantEffects)
                    {
                        if (effect.InstantEffectType == InstantEffectType.ReduceHunger)
                        {
                            _unitsService.HeroParameters.HungerParameter.AddCurrent(effect.Value, true);
                        }
                    }
                }
                _unitsService.HeroStorage.Items.Remove(model);

                break;
            }
        }
    }
}