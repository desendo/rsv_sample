using Game.Services;

namespace Game.Rules
{
    public class CancelHeroJobRule
    {
        private readonly UnitsService _unitsService;

        public CancelHeroJobRule(UnitsService unitsService)
        {
            _unitsService = unitsService;
            _unitsService.Hero.WayPoint.Subscribe(x => OnWayPointChanged());
            _unitsService.Hero.HasWayPoint.Subscribe(x => OnWayPointAppearanceChanged());
        }

        private void OnWayPointChanged()
        {
            _unitsService.Hero.CurrentJob.Value = null;
        }

        private void OnWayPointAppearanceChanged()
        {
            if(!_unitsService.Hero.HasWayPoint.Value)
                _unitsService.Hero.CurrentJob.Value = null;
        }
    }
}