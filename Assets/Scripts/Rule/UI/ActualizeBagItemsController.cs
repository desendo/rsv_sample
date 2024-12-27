using Game.Services;
using Game.Signals;
using Modules.Common;

namespace Game.Rules.UI
{
    public class ActualizeBagItemsRule
    {
        private readonly HeroService _heroService;

        public ActualizeBagItemsRule(ISignalBus signalBus, HeroService heroService)
        {
            _heroService = heroService;
            signalBus.Subscribe<UIViewSignals.ActualizeBagItemPositionRequest>(HandleActualizeBagItemPositionRequest);
        }

        private void HandleActualizeBagItemPositionRequest(UIViewSignals.ActualizeBagItemPositionRequest obj)
        {
            foreach (var model in _heroService.HeroStorage.Items)
                if (obj.Uid == model.UId)
                {
                    model.ViewPosition.Value = obj.Position;
                    model.ViewRotation.Value = obj.Rotation.eulerAngles.z;
                    break;
                }
        }
    }
}