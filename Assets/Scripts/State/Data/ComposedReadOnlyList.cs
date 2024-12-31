using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Game.State.Data
{
    public class ComposedReadOnlyList<T> : IReadOnlyList<T>
    {
        private readonly IReadOnlyList<List<T>> _lists;

        public ComposedReadOnlyList(params List<T>[] lists)
        {
            _lists = lists;
        }

        public ComposedReadOnlyList(IReadOnlyList<List<T>> lists)
        {
            _lists = lists;
        }

        public T this[int index]
        {
            get
            {
                foreach (var list in _lists)
                {
                    if (index < list.Count) return list[index];
                    index -= list.Count;
                }

                throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        public int Count => _lists.Sum(list => list.Count);

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var list in _lists)
            foreach (var item in list)
                yield return item;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}