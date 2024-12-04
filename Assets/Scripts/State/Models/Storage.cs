using Game.State.Data;
using Modules.Reactive.Actions;
using Modules.Reactive.Values;
using UnityEngine;

namespace Game.State.Models
{
    public class StorageModel
    {
        public readonly ReactiveCollection<ItemModel> Items = new();

        public void AddItem(string Id)
        {
            Items.Add(new ItemModel
            {
                TypeId =
                {
                    Value = Id
                },
                UId = StateData.GenerateUid(),
                ViewPosition = { Value = new Vector2(Random.value  * 0.1f, 0)}
            });
        }
    }

    public class ItemModel : IModel
    {
        public IReactiveEvent PreSave { get; } = new ReactiveEvent();
        public IReactiveVariable<float> ViewRotation { get; } = new ReactiveVariable<float>();
        public IReactiveVariable<Vector2> ViewPosition { get; } = new ReactiveVariable<Vector2>();
        public int UId { get; set; }
        public IReactiveVariable<string> TypeId { get; } = new ReactiveVariable<string>();
    }
}