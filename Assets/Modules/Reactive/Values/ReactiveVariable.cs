using Modules.Reactive.Actions;
using System;
using UnityEngine;

namespace Modules.Reactive.Values
{

    public class ReactiveVariable<T> : IReactiveVariable<T>
    {
        private T value;
        private T previousValue;

        public ReactiveVariable()
        {
            this.value = default;
        }

        public ReactiveVariable(T value)
        {
            if (value == null)
                value = default(T);
            this.value = value;
            this.previousValue = value;
        }

        public ReactiveEvent<T> OnChanged { get; set; } = new();

        public T Value
        {
            get => this.value;
            set
            {
                this.value = value;
                if (!Equals(this.value, this.previousValue))
                {
                    this.previousValue = this.value;
                    this.OnChanged?.Invoke(value);
                }
                this.previousValue = this.value;
            }
        }

        public void SetForceNotify(T val)
        {
            this.value = val;
            this.previousValue = this.value;
            this.OnChanged?.Invoke(val);
        }

        public IDisposable Subscribe(Action<T> callback)
        {
            callback.Invoke(this.Value);
            return this.OnChanged.Subscribe(callback);
        }
    }
}