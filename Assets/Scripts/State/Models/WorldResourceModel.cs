using Modules.Reactive.Values;
using UnityEngine;

namespace Game.State.Models
{
    public class WorldResourceModel : ISelectableModel, IWorldModel, IJobModel
    {
        public string ItemType { get; set; }

        public IReactiveVariable<int> Count { get; } = new ReactiveVariable<int>();
        public IReactiveVariable<int> Capacity { get; } = new ReactiveVariable<int>();
        public int UId { get; set; }
        public IReactiveVariable<string> TypeId { get; } = new ReactiveVariable<string>();
        public IReactiveVariable<bool> Selected { get; } = new ReactiveVariable<bool>();
        public IReactiveVariable<bool> Hovered { get; } = new ReactiveVariable<bool>();


        public IReactiveVariable<Vector3> Position { get; } = new ReactiveVariable<Vector3>();
        public IReactiveVariable<Quaternion> Rotation { get; } = new ReactiveVariable<Quaternion>();
    }
}