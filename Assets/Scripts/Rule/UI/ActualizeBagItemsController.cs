using Game.Services;
using Game.Signals;
using Modules.Common;

namespace Game.Rules.UI
{
    public class ActualizeBagItemsRule
    {
        private readonly UnitsService _unitsService;

        public ActualizeBagItemsRule(ISignalBus signalBus, UnitsService unitsService)
        {
            _unitsService = unitsService;
            signalBus.Subscribe<UIViewSignals.ActualizeBagItemPositionRequest>(HandleActualizeBagItemPositionRequest);
        }

        private void HandleActualizeBagItemPositionRequest(UIViewSignals.ActualizeBagItemPositionRequest obj)
        {
            foreach (var model in _unitsService.HeroStorage.Items)
                if (obj.Uid == model.UId)
                {
                    model.ViewPosition.Value = obj.Position;
                    model.ViewRotation.Value = obj.Rotation.eulerAngles.z;
                    break;
                }
        }
    }
}