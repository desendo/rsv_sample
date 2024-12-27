using Game.Services;

namespace Game.Rules
{
    public class CancelHeroJobRule
    {
        private readonly HeroService _heroService;

        public CancelHeroJobRule(HeroService heroService)
        {
            _heroService = heroService;
            _heroService.Hero.WayPoint.Subscribe(x => OnWayPointChanged());
            _heroService.Hero.HasWayPoint.Subscribe(x => OnWayPointAppearanceChanged());
        }

        private void OnWayPointChanged()
        {
            _heroService.Hero.CurrentJob.Value = null;
        }

        private void OnWayPointAppearanceChanged()
        {
            if(!_heroService.Hero.HasWayPoint.Value)
                _heroService.Hero.CurrentJob.Value = null;
        }
    }
}