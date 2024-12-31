using System.Linq;
using Game.Services;
using Game.Signals;
using Modules.Common;

namespace Game.Rules.Infrastructure
{
    public class LocalizationLoadRule
    {
        public LocalizationLoadRule(GameConfig gameConfig, SignalBus signalBus)
        {
            LocService.Init(gameConfig.Localization.Languages[0]);
            signalBus.Subscribe<UIViewSignals.SetLanguageRequest>(request =>
            {
                var targetLang = gameConfig.Localization.Languages.FirstOrDefault(x => x.Id == request.Lang);
                if (targetLang != null) LocService.Init(targetLang);
            });
        }
    }
}