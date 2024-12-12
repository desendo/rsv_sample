using Modules.Reactive.Values;

namespace Game.State.Models
{
    public class StorageModel
    {
        public IReactiveCollection<StorageItemModel> Items { get; } = new ReactiveCollection<StorageItemModel>();
        public IReactiveVariable<float> Width { get; } = new ReactiveVariable<float>();
        public IReactiveVariable<float> Height { get; } = new ReactiveVariable<float>();


    }
}