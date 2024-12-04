using Game.Services;
using Game.Signals;
using Modules.Common;

namespace Game.Controllers.Infrastructure
{
    public class PauseController
    {
        public PauseController(IUpdateProvider updateProvider, ISignalBus signalBus)
        {
            signalBus.Subscribe<UIViewSignals.TogglePause>(pause =>
            {
                updateProvider.SetPaused(!updateProvider.Paused.Value);
            });
        }
    }
}