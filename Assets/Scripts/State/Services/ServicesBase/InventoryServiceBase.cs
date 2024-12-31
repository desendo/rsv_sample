using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Modules.Reactive.Actions;
using Modules.Reactive.Values;
using UnityEngine;

namespace Game.Services.ServicesBase
{
    public abstract class InventoryServiceBase<T> : IReactiveCollection<T>
    {
        private static readonly Dictionary<Type, Type[]> InterfaceCache = new();
        private readonly IReactiveCollection<T> _implementation = new ReactiveCollection<T>();


        private readonly Dictionary<Type, List<object>> _interfaceCollections = new();

        protected InventoryServiceBase()
        {
            foreach (var @interface in GetCachedInterfaces(typeof(T)))
                _interfaceCollections.Add(@interface, new List<object>());
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _implementation.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_implementation).GetEnumerator();
        }

        public void Add(T item)
        {
            foreach (var @interface in GetCachedInterfaces(typeof(T)))
                _interfaceCollections[@interface].Add(item);
            _implementation.Add(item);
        }

        public void Clear()
        {
            foreach (var (key, value) in _interfaceCollections) value.Clear();

            _implementation.Clear();
        }

        public bool Contains(T item)
        {
            return _implementation.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _implementation.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            foreach (var @interface in GetCachedInterfaces(typeof(T)))
                _interfaceCollections[@interface].Remove(item);
            return _implementation.Remove(item);
        }

        public int Count => _implementation.Count;

        public bool IsReadOnly => _implementation.IsReadOnly;

        public ReactiveEvent<ICollection<T>> OnAnyEvent
        {
            get => _implementation.OnAnyEvent;
            set => _implementation.OnAnyEvent = value;
        }

        public ReactiveEvent<T> OnAdd
        {
            get => _implementation.OnAdd;
            set => _implementation.OnAdd = value;
        }

        public ReactiveEvent OnClear
        {
            get => _implementation.OnClear;
            set => _implementation.OnClear = value;
        }

        public ReactiveEvent<T> OnRemove
        {
            get => _implementation.OnRemove;
            set => _implementation.OnRemove = value;
        }

        public ReactiveEvent<T> OnChange
        {
            get => _implementation.OnChange;
            set => _implementation.OnChange = value;
        }

        public ReactiveEvent<List<T>> OnNew
        {
            get => _implementation.OnNew;
            set => _implementation.OnNew = value;
        }

        public IDisposable Subscribe(Action<List<T>, IReactiveCollection<T>.EventType> callback)
        {
            return _implementation.Subscribe(callback);
        }

        public void Set(List<T> elements)
        {
            _implementation.Set(elements);
        }

        protected IEnumerable<TInterface> GetByInterface<TInterface>() where TInterface : class
        {
            if (_interfaceCollections.TryGetValue(typeof(TInterface), out var list)) return list.Cast<TInterface>();
            Debug.LogWarning($"empty Enumerable for {typeof(TInterface)} aspect of {typeof(T)} ");
            return Enumerable.Empty<TInterface>();
        }

        private static Type[] GetCachedInterfaces(Type type)
        {
            if (!InterfaceCache.TryGetValue(type, out var interfaces))
            {
                interfaces = type.GetInterfaces();
                InterfaceCache[type] = interfaces;
            }

            return interfaces;
        }
    }
}