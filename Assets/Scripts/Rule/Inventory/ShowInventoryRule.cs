using Game.Services;
using Game.Signals;
using Modules.Common;

namespace Game.Rule.Inventory
{
    public class ShowInventoryRule
    {
        private readonly UnitsService _unitsService;

        public ShowInventoryRule(SignalBus signalBus, UnitsService unitsService)
        {
            _unitsService = unitsService;
            signalBus.Subscribe<UIViewSignals.ToggleInventoryShownRequest>(HandleToggleInventoryRequest);
        
        }

        

        private void HandleToggleInventoryRequest(UIViewSignals.ToggleInventoryShownRequest obj)
        {
            _unitsService.Hero.InventoryShown.Value = !_unitsService.Hero.InventoryShown.Value;
        }
    }
}