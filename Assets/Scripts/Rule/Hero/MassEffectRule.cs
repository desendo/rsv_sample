using Game.Services;

namespace Game.Rules
{
    public class MassEffectRule
    {
        private readonly HeroService _heroService;

        public MassEffectRule(HeroService heroService)
        {
            _heroService = heroService;
            _heroService.HeroParameters.MassParameter.NormalizedUnclamped.Subscribe(HandleMass);
        }

        private void HandleMass(float normalizedValue)
        {
            if (normalizedValue > 1f)
            {
                _heroService.HeroParameters.MoveSpeedFactor.Value = 1 / normalizedValue;
                _heroService.Hero.Say($"Перегрузка. Скорость {(int)(_heroService.HeroParameters.MoveSpeedFactor.Value * 100)}%");

            }
            else
            {
                if(_heroService.HeroParameters.MoveSpeedFactor.Value < 1f)
                    _heroService.Hero.Say($"Перегрузки больше нет");
                _heroService.HeroParameters.MoveSpeedFactor.Value = 1f;

            }

        }

    }
}