using Game.Services;
using Game.Signals;
using Modules.Common;

namespace Game.Rules.Infrastructure
{
    public class PauseRule
    {
        public PauseRule(IUpdateProvider updateProvider, ISignalBus signalBus)
        {
            signalBus.Subscribe<UIViewSignals.TogglePause>(pause =>
            {
                updateProvider.SetPaused(!updateProvider.Paused.Value);
            });
        }
    }
}