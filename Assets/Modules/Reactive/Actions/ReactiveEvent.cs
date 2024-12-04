using Modules.Reactive.Values;

namespace Modules.Reactive.Actions
{
    using System;
    using System.Collections.Generic;

    public interface IReactiveEvent : IReadOnlyReactiveEvent
    {
        void Invoke();
    }
    public interface IReadOnlyReactiveEvent
    {
        IDisposable Subscribe(Action callback);

        void UnSubscribe(Action callback);
    }
    public interface IReactiveEvent<T1,T2> : IReadOnlyReactiveEvent<T1, T2>
    {
        void Invoke((T1,T2) val);
    }
    public interface IReadOnlyReactiveEvent<T>
    {
        IDisposable Subscribe(Action<T> callback);

        void UnSubscribe(Action<T> callback);
    }
    public interface IReactiveEvent<T> : IReadOnlyReactiveEvent<T>
    {
        void Invoke(T val);
    }

    public interface IReadOnlyReactiveEvent<out T1, out T2>
    {
        IDisposable Subscribe(Action<T1,T2> callback);

        void UnSubscribe(Action<T1,T2> callback);
    }
    public sealed class ReactiveEvent : IReactiveAction, IReactiveEvent
    {
        private readonly List<Action> actions = new();

        public void Invoke()
        {
            foreach (var action in this.actions)
            {
                action.Invoke();
            }
        }

        public IDisposable Subscribe(Action callback)
        {
            this.actions.Add(callback);
            return new DisposeContainer(() => this.UnSubscribe(callback));
        }

        public void UnSubscribe(Action callback)
        {
            this.actions.Remove(callback);
        }
    }

    public class ReactiveEvent<T> : IAction<T>, IReactiveEvent<T>
    {
        private readonly List<IAction<T>> actions = new();

        private readonly List<IAction<T>> cache = new();

        private readonly Dictionary<Action<T>, IAction<T>> delegates = new();

        public void Invoke(T args)
        {
            this.cache.Clear();
            this.cache.AddRange(this.actions);

            for (int i = 0, count = this.cache.Count; i < count; i++)
            {
                var action = this.cache[i];
                action.Invoke(args);
            }
        }

        public IDisposable Subscribe(Action<T> callback)
        {
            var action = new ReactiveAction<T>(callback);
            this.actions.Add(action);
            this.delegates[callback] = action;
            return new DisposeContainer(() => this.UnSubscribe(callback));
        }

        public void UnSubscribe(Action<T> callback)
        {
            if (this.delegates.TryGetValue(callback, out var action))
            {
                this.delegates.Remove(callback);
                this.actions.Remove(action);
            }
        }
    }
    public class ReactiveEvent<T1,T2> : IAction<T1,T2>, IReactiveEvent<T1,T2>
    {
        private readonly List<IAction<T1,T2>> actions = new();


        private readonly Dictionary<Action<T1,T2>, IAction<T1,T2>> delegates = new();

        public void Invoke(T1 arg1, T2 arg2)
        {
            foreach (var action in actions)
            {
                action.Invoke(arg1, arg2);
            }
        }

        public IDisposable Subscribe(Action<T1,T2> callback)
        {
            var action = new ReactiveAction<T1,T2>(callback);
            this.actions.Add(action);
            this.delegates[callback] = action;
            return new DisposeContainer(() => this.UnSubscribe(callback));
        }

        public void UnSubscribe(Action<T1,T2> callback)
        {
            if (this.delegates.TryGetValue(callback, out var action))
            {
                this.delegates.Remove(callback);
                this.actions.Remove(action);
            }
        }

        public void Invoke((T1, T2) val)
        {
            foreach (var action in actions)
            {
                action.Invoke(val.Item1, val.Item2);
            }

        }
    }
}