using System;
using System.Collections.Generic;
using Modules.Reactive.Actions;

namespace Modules.Reactive.Values
{
    public interface IReactiveCollection<T> : ICollection<T>
    {
        public enum EventType
        {
            Add,
            Remove,
            New,
            Clear,
            ChangeElement
        }
        ReactiveEvent<T> OnAdd { get; set; }
        ReactiveEvent OnClear { get; set; }
        ReactiveEvent<T> OnRemove { get; set; }
        ReactiveEvent<T> OnChange { get; set; }
        ReactiveEvent<List<T>> OnNew { get; set; }
        public IDisposable Subscribe(Action<List<T>, EventType> callback);
        void Set(List<T> elements);
    }
}