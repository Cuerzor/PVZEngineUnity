#nullable enable

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PVZEngine.Tools
{
    public class Grouping<TKey, TElement> : IGrouping<TKey, TElement>
    {
        public void Add(TElement element)
        {
            list.Add(element);
        }
        public bool Remove(TElement element)
        {
            return list.Remove(element);
        }
        public void Clear()
        {
            list.Clear();
        }
        public IEnumerator<TElement> GetEnumerator()
        {
            return list.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public int Count => list.Count;
        public TElement this[int index] => list[index];
        public TKey Key { get; internal set; } = default!;
        private List<TElement> list = new List<TElement>();
    }
}