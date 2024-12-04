using System;

namespace Modules.Reactive.Actions
{

    public sealed class ReactiveAction : IReactiveAction
    {
        private readonly Action action;

        public ReactiveAction(Action action)
        {
            this.action = action;
        }

        public void Invoke()
        {
            this.action?.Invoke();
        }
    }

    public sealed class ReactiveAction<T> : IAction<T>
    {
        private readonly Action<T> action;

        public ReactiveAction(Action<T> action)
        {
            this.action = action;
        }

        public void Invoke(T args)
        {
            this.action?.Invoke(args);
        }
    }

    public sealed class ReactiveAction<T1, T2> : IAction<T1, T2>
    {
        private readonly Action<T1, T2> action;

        public ReactiveAction(Action<T1, T2> action)
        {
            this.action = action;
        }

        public void Invoke(T1 arg1, T2 arg2)
        {
            this.action?.Invoke(arg1, arg2);
        }
    }
}