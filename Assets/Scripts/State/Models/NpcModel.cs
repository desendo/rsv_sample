using Modules.Reactive.Values;
using UnityEngine;

namespace Game.State.Models
{
    public class NpcModel : IModel, ISelectableModel, IWorldModel, IInteractionModel
    {
        public string DialogConfigId { get; set; }
        public int DialogUId { get; set; }
        public int UId { get; set; }
        public IReactiveVariable<string> TypeId { get; } = new ReactiveVariable<string>();
        public IReactiveVariable<bool> Selected { get; } = new ReactiveVariable<bool>();
        public IReactiveVariable<bool> Hovered { get; } = new ReactiveVariable<bool>();
        public IReactiveVariable<Vector3> Position { get; } = new ReactiveVariable<Vector3>();
        public IReactiveVariable<Quaternion> Rotation { get; } = new ReactiveVariable<Quaternion>();
    }
}