using Game.Services;

namespace Game.Rules
{
    public class MassEffectRule
    {
        private readonly UnitsService _unitsService;

        public MassEffectRule(UnitsService unitsService)
        {
            _unitsService = unitsService;
            _unitsService.HeroParameters.MassParameter.NormalizedUnclamped.Subscribe(HandleMass);
        }

        private void HandleMass(float normalizedValue)
        {
            if (normalizedValue > 1f)
            {
                _unitsService.HeroParameters.MoveSpeedFactor.Value = 1 / normalizedValue;
                _unitsService.Hero.Say($"Перегрузка. Скорость {(int)(_unitsService.HeroParameters.MoveSpeedFactor.Value * 100)}%");

            }
            else
            {
                if(_unitsService.HeroParameters.MoveSpeedFactor.Value < 1f)
                    _unitsService.Hero.Say($"Перегрузки больше нет");
                _unitsService.HeroParameters.MoveSpeedFactor.Value = 1f;

            }

        }

    }
}