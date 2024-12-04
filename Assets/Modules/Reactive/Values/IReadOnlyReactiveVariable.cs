using Modules.Reactive.Actions;
using System;
namespace Modules.Reactive.Values
{


    public interface IReadOnlyReactiveVariable<T> : IReactiveValue<T>
    {
        ReactiveEvent<T> OnChanged { get; set; }

        new T Value { get; }

        public IDisposable Subscribe(Action<T> callback);
    }
}