using Game.Services;
using Modules.Reactive.Values;

namespace Game.State.Models
{
    public class HeroParameters
    {
        public HeroParameters()
        {
            HungerParameter.BindTitle(LocService.ById(nameof(HungerParameter)));
            MassParameter.BindTitle(LocService.ById(nameof(MassParameter)));
        }

        public IReactiveVariable<float> MaxMoveSpeed { get; } = new ReactiveVariable<float>(1);
        public IReactiveVariable<float> MoveSpeedFactor { get; } = new ReactiveVariable<float>(1);
        public LimitedFloatParameter HungerParameter { get; } = new();
        public LimitedFloatParameter MassParameter { get; } = new();
    }
}