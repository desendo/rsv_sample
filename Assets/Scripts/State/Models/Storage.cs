using Game.State.Data;
using Modules.Reactive.Values;
using UnityEngine;

namespace Game.State.Models
{
    public class StorageModel
    {
        public IReactiveCollection<StorageItemModel> Items { get; } = new ReactiveCollection<StorageItemModel>();
        public IReactiveVariable<float> Mass { get; }  = new ReactiveVariable<float>();


    }
}