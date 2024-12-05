using Game.Services;
using UnityEngine;

namespace Game.Controllers
{
    public class MassEffectController
    {
        private readonly HeroService _heroService;

        public MassEffectController(HeroService heroService)
        {
            _heroService = heroService;
            _heroService.HeroStorage.Mass.Subscribe(x=>HandleMass());
        }

        private void HandleMass()
        {
            var delta = _heroService.HeroParameters.MaxMass.Value - _heroService.HeroStorage.Mass.Value;
            if (delta >= 0)
            {
                _heroService.HeroParameters.MoveSpeedFactor.Value = 1f;
            }
            else
            {
                _heroService.HeroParameters.MoveSpeedFactor.Value =
                    _heroService.HeroParameters.MaxMass.Value / _heroService.HeroStorage.Mass.Value;
                _heroService.Hero.Say($"Тяжело");
            }
        }

    }
}