using Modules.Reactive.Values;
using UnityEngine;

namespace Game.State.Models
{
    public interface IModel
    {
        public int UId { get; }
        public IReactiveVariable<string> TypeId { get; }
    }

    public interface IWorldModel : IModel
    {
        public IReactiveVariable<Vector3> Position { get; }
        public IReactiveVariable<Quaternion> Rotation { get; }
    }

    public interface IJobModel : IModel
    {
    }

    public interface ISelectableModel : IModel
    {
        IReactiveVariable<bool> Selected { get; }
        IReactiveVariable<bool> Hovered { get; }
    }
}