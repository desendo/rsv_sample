using System.Collections;
using System.Collections.Generic;
using Modules.Reactive.Actions;

namespace Modules.Reactive.Values
{
    public interface IReactiveDictionary<T1,T2> : IReadOnlyDictionary<T1, T2>
    {
        ReactiveEvent<T1,T2> OnAdd { get; set; }
        ReactiveEvent<T1,T2> OnRemove { get; set; }
        ReactiveEvent<T1,T2> OnChange { get; set; }
        public void SetOrAddValue(T1 key, T2 newValue);
    }

    public class ReactiveDictionary<T1, T2> : IReactiveDictionary<T1, T2>
    {
        private readonly Dictionary<T1, T2> dictionary = new();

        public int Count => dictionary.Count;
        public bool IsReadOnly => false;
        public ReactiveEvent<T1, T2> OnAdd { get; set; } = new();
        public ReactiveEvent<T1, T2> OnRemove { get; set; } = new();
        public ReactiveEvent<T1, T2> OnChange { get; set; } = new();
        public void SetOrAddValue(T1 key, T2 newValue)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = newValue;
                OnChange.Invoke(key,dictionary[key]);
            }
            else
            {
                Add(key, newValue);
            }

        }

        public IEnumerator<KeyValuePair<T1, T2>> GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T1 key, T2 item)
        {
            dictionary.Add(key,item);
            OnAdd.Invoke(key, item);
        }

        public void Clear()
        {
            foreach (var keyValuePair in dictionary)
            {
                OnRemove.Invoke(keyValuePair.Key, keyValuePair.Value);
            }

            dictionary.Clear();
        }

        public bool Remove(T1 key, T2 item)
        {
            if (dictionary.Remove(key))
            {
                OnRemove.Invoke(key, item);
                return true;
            }

            return false;
        }


        public bool ContainsKey(T1 key)
        {
            return dictionary.ContainsKey(key);
        }

        public bool TryGetValue(T1 key, out T2 value)
        {
            return dictionary.TryGetValue(key, out value);
        }

        public T2 this[T1 key] => dictionary[key];

        public IEnumerable<T1> Keys => dictionary.Keys;
        public IEnumerable<T2> Values => dictionary.Values;
    }
}