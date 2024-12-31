using Game.State.Data;
using Game.State.Models;
using Modules.Reactive.Values;

namespace Game.Services
{
    public class PhysicsService : ISaveLoadData<StateData>, ILoadData<GameConfig>
    {
        public IReactiveVariable<bool> IsPhysics2dUpdateEnabled { get; } = new ReactiveVariable<bool>();
        public IReactiveVariable<float> PhysicsGBase { get; } = new ReactiveVariable<float>();
        public IReactiveVariable<float> PhysicsGMultiplier { get; } = new ReactiveVariable<float>();

        public void LoadFrom(in GameConfig data)
        {
            PhysicsGBase.Value = data.Gravity2d;
        }

        public void LoadFrom(in StateData data)
        {
            IsPhysics2dUpdateEnabled.Value = data.Physics.Is2dEnabled;
            PhysicsGMultiplier.Value = data.Physics.PhysicsGMultiplier;
        }

        public void SaveTo(StateData data)
        {
            data.Physics.Is2dEnabled = IsPhysics2dUpdateEnabled.Value;
            data.Physics.PhysicsGMultiplier = PhysicsGMultiplier.Value;
        }
    }
}