using System.Collections.Generic;
using Game.Services;
using Game.State.Models;

namespace Game.Rules
{
    public class CalculateItemsMassRule
    {
        private readonly HeroService _heroService;

        public CalculateItemsMassRule(HeroService heroService)
        {
            _heroService = heroService;
            heroService.HeroStorage.Items.OnAnyEvent.Subscribe(UpdateMass);
            UpdateMass(heroService.HeroStorage.Items);
        }

        private void UpdateMass(ICollection<StorageItemModel> modelsCollection)
        {
            var mass = 0f;
            foreach (var model in modelsCollection) mass += model.Mass.Value;
            _heroService.HeroParameters.MassParameter.SetCurrent(mass);
        }
    }
}