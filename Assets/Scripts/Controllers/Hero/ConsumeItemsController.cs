using Game.Services;
using Game.Signals;
using Game.State.Models;
using Modules.Common;

namespace Game.Controllers
{
    public class ConsumeItemsController
    {
        private readonly HeroService _heroService;
        private readonly GameConfig _gameConfig;

        public ConsumeItemsController(ISignalBus signalBus, HeroService heroService, GameConfig gameConfig)
        {
            _heroService = heroService;
            _gameConfig = gameConfig;
            signalBus.Subscribe<UIViewSignals.ConsumeItemHeroStorageRequest>(HandleConsumeItemHeroStorageRequest);
        }

        private void HandleConsumeItemHeroStorageRequest(UIViewSignals.ConsumeItemHeroStorageRequest obj)
        {
            StorageItemModel model = null;
            foreach (var x in _heroService.HeroStorage.Items)
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

                    _heroService.Hero.Say("Использовал "+_gameConfig.Localization.GetObjectTitle(model.TypeId.Value));
                    foreach (var effect in result.InstantEffects)
                    {
                        if (effect.InstantEffectType == InstantEffectType.ReduceHunger)
                        {
                            _heroService.HeroParameters.Hunger.Current.Value += effect.Value;
                        }
                    }
                }
                _heroService.HeroStorage.Items.Remove(model);

                break;
            }
        }
    }
}