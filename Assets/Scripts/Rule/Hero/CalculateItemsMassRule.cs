using System.Collections.Generic;
using Game.Services;
using Game.State.Models;

namespace Game.Rules
{
    public class CalculateItemsMassRule
    {
        private readonly UnitsService _unitsService;

        public CalculateItemsMassRule(UnitsService unitsService)
        {
            _unitsService = unitsService;
            unitsService.HeroStorage.Items.OnAnyEvent.Subscribe(UpdateMass);
            UpdateMass(unitsService.HeroStorage.Items);
        }

        private void UpdateMass(ICollection<StorageItemModel> modelsCollection)
        {
            var mass = 0f;
            foreach (var model in modelsCollection)
            {
                mass += model.Mass.Value;
            }
            _unitsService.HeroParameters.MassParameter.SetCurrent(mass);
        }
    }
}