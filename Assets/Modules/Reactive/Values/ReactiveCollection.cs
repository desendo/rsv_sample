using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Modules.Reactive.Actions;

namespace Modules.Reactive.Values
{
    public class ReactiveCollection<T> : IReactiveCollection<T>
    {
        private ICollection<T>  value;
        private readonly Dictionary<Action,Action> callbacks = new Dictionary<Action, Action>();
        public int Count => value.Count;
        public bool IsReadOnly => value.IsReadOnly;
        public ReactiveEvent OnClear { get; set; } = new ReactiveEvent();
        public ReactiveEvent<T> OnAdd { get; set; } = new ReactiveEvent<T>();
        public ReactiveEvent<T> OnRemove { get; set; } = new ReactiveEvent<T>();
        public ReactiveEvent<T> OnChange { get; set; } = new ReactiveEvent<T>();
        public ReactiveEvent<List<T>> OnNew { get; set; } = new ReactiveEvent<List<T>>();
        public ReactiveCollection()
        {
            this.value = new List<T>();
        }

        public ReactiveCollection(ICollection<T> value)
        {
            this.value = value;

        }

        public IEnumerator<T> GetEnumerator()
        {
            return value.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            value.Add(item);
            OnAdd.Invoke(item);
        }

        public void Clear()
        {
            value.Clear();
            OnClear.Invoke();
        }

        public bool Contains(T item)
        {
            return value.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            value.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            var result = value.Remove(item);
            if(result)
                OnRemove.Invoke(item);
            return result;
        }


        public IDisposable Subscribe(Action<List<T>, IReactiveCollection<T>.EventType> callback)
        {
            callback.Invoke(this.value.ToList(), IReactiveCollection<T>.EventType.New);
            var add = OnAdd.Subscribe(obj =>
                callback.Invoke(new List<T> { obj }, IReactiveCollection<T>.EventType.Add));
            var remove = OnRemove.Subscribe(obj =>
                callback.Invoke(new List<T> { obj }, IReactiveCollection<T>.EventType.Remove));
            var clear = OnClear.Subscribe(() =>
                callback.Invoke(null, IReactiveCollection<T>.EventType.Clear));
            var onNew = OnNew.Subscribe(obj =>
                callback.Invoke(this.value.ToList(), IReactiveCollection<T>.EventType.New));
            var onChange = OnChange.Subscribe(obj =>
                callback.Invoke(new List<T> {obj }, IReactiveCollection<T>.EventType.ChangeElement));
            return new DisposeContainer(()=>
            {
                add.Dispose();
                remove.Dispose();
                onNew.Dispose();
                clear.Dispose();
                onChange.Dispose();
            });
        }

        public void Set(List<T> elements)
        {
            this.value = elements;
            OnNew.Invoke(this.value.ToList());
        }
    }
}