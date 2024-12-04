using System.Linq;
using Game.Services;
using Game.Signals;
using Modules.Common;

namespace Game.Controllers
{
    public class DropItemsController
    {
        private readonly HeroService _heroService;

        public DropItemsController(SignalBus signalBus, HeroService heroService)
        {
            _heroService = heroService;
            signalBus.Subscribe<UIViewSignals.DropItemHeroStorageRequest>(HandleDropItemHeroStorageRequest);
        }

        private void HandleDropItemHeroStorageRequest(UIViewSignals.DropItemHeroStorageRequest obj)
        {
            var model = _heroService.HeroStorage.Items.FirstOrDefault(x => x.UId == obj.Uid);
            if (model != null)
            {
                _heroService.HeroStorage.Items.Remove(model);
                _heroService.Hero.Say("Что-то упало - чемодан полный!");
            }
        }
    }
}