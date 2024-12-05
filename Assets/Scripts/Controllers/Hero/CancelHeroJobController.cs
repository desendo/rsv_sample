using Game.Services;

namespace Game.Controllers
{
    public class CancelHeroJobController
    {
        private readonly HeroService _heroService;

        public CancelHeroJobController(HeroService heroService)
        {
            _heroService = heroService;
            _heroService.Hero.HasWayPoint.Subscribe(OnWayPointAppear);
        }

        private void OnWayPointAppear(bool hasWayPoint)
        {
            if (!hasWayPoint)
                return;

            _heroService.Hero.CurrentJob.Value = null;
        }
    }
}