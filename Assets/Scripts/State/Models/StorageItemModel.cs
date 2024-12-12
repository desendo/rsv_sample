using Modules.Reactive.Actions;
using Modules.Reactive.Values;
using UnityEngine;

namespace Game.State.Models
{
    public class StorageItemModel : IModel
    {
        public IReactiveVariable<float> Scale { get; } = new ReactiveVariable<float>();
        public IReactiveEvent PreSave { get; } = new ReactiveEvent();
        public IReactiveVariable<float> ViewRotation { get; } = new ReactiveVariable<float>();
        public IReactiveVariable<float> Mass { get; } = new ReactiveVariable<float>();
        public IReactiveVariable<Vector2> ViewPosition { get; } = new ReactiveVariable<Vector2>();
        public int UId { get; set; }
        public IReactiveVariable<string> TypeId { get; } = new ReactiveVariable<string>();
    }
}