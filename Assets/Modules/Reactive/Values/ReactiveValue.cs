using System;

namespace Modules.Reactive.Values
{

    public sealed class ReactiveValue<T> : IReactiveValue<T>
    {
        private readonly Func<T> func;

        public ReactiveValue(Func<T> func)
        {
            this.func = func;
        }

        public T Value => this.func.Invoke();
    }
}