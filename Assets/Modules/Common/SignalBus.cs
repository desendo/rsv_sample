using System;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.Common
{
    public static class DisposableExt
    {
        public static void AddTo(this IDisposable disposable, List<IDisposable> disposables)
        {
            disposables.Add(disposable);
        }
        public static void DisposeAndClear(this List<IDisposable> disposables)
        {
            foreach (var disposable in disposables)
            {
                disposable.Dispose();
            }
            disposables.Clear();
        }
    }

    public class SignalBus : ISignalBus
    {
        private readonly HashSet<Type> currentlyFiring = new();

        private readonly Dictionary<Type, List<object>> subscriptions = new();

        private readonly Dictionary<Type, List<object>> unsubscribeBuffer = new();

        public void Fire<T>(T signal)
        {
            var type = typeof(T);
            if (!this.subscriptions.TryGetValue(type, out var callbacks))
            {
                Debug.LogWarning("No subscribers to " + typeof(T).Name);
                return;
            }

            this.currentlyFiring.Add(type);

            foreach (var obj in callbacks)
            {
                if (obj is Action<T> callback)
                {
                    callback.Invoke(signal);
                }
            }

            if (this.unsubscribeBuffer.TryGetValue(type, out var value))
            {
                foreach (var obj in value)
                {
                    callbacks.Remove(obj);
                }
            }

            this.currentlyFiring.Remove(typeof(T));
        }

        public IDisposable Subscribe<T>(Action<T> callback)
        {
            var type = typeof(T);
            if (!this.subscriptions.ContainsKey(type))
            {
                this.subscriptions.Add(typeof(T), new List<object>());
            }

            this.subscriptions[typeof(T)].Add(callback);
            return new DisposeContainer(() => this.DisposeCallback<T>(callback));
        }

        public void UnSubscribe<T>(Action<T> callback)
        {
            var type = typeof(T);
            if (!this.currentlyFiring.Contains(type)
                && this.subscriptions.TryGetValue(type, out var subscription))
            {
                subscription.Remove(callback);
            }
            else
            {
                if (!this.unsubscribeBuffer.ContainsKey(type))
                {
                    this.unsubscribeBuffer.Add(type, new List<object>());
                }

                this.unsubscribeBuffer[type].Add(callback);
            }
        }

        private void DisposeCallback<T>(object callback)
        {
            if (callback is Action<T> action)
            {
                this.UnSubscribe(action);
            }
            else
            {
                throw new Exception($"SignalBus.DisposeCallback: callback is not {typeof(Action<T>)}");
            }
        }
    }

    public interface ISignalBus
    {
        void Fire<T>(T signal);
        IDisposable Subscribe<T>(Action<T> callback);
    }
}