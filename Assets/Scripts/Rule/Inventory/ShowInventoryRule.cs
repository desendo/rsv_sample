using Game.Services;
using Game.Signals;
using Modules.Common;

namespace Game.Rule.Inventory
{
    public class ShowInventoryRule
    {
        private readonly HeroService _heroService;

        public ShowInventoryRule(SignalBus signalBus, HeroService heroService)
        {
            _heroService = heroService;
            signalBus.Subscribe<UIViewSignals.ToggleInventoryShownRequest>(HandleToggleInventoryRequest);
        
        }

        

        private void HandleToggleInventoryRequest(UIViewSignals.ToggleInventoryShownRequest obj)
        {
            _heroService.Hero.InventoryShown.Value = !_heroService.Hero.InventoryShown.Value;
        }
    }
}