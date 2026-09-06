#nullable enable

using System.Collections;
using System.Collections.Generic;

namespace PVZEngine.Tools
{
    public class Group<TKey, TElement> : IEnumerable<Grouping<TKey, TElement>>
    {
        public void Add(TKey key, TElement element)
        {
            foreach (var grouping in groupings)
            {
                if (keyEqualityComparer.Equals(grouping.Key, key))
                {
                    grouping.Add(element);
                    return;
                }
            }
            var newGrouping = GroupingPool<TKey, TElement>.Get(key);
            newGrouping.Add(element);
            groupings.Add(newGrouping);
        }
        public bool Remove(TKey key, TElement element)
        {
            int index = -1;
            Grouping<TKey, TElement>? grouping = null;
            for (int i = 0; i < groupings.Count; i++)
            {
                if (keyEqualityComparer.Equals(groupings[i].Key, key))
                {
                    index = i;
                    grouping = groupings[i];
                    break;
                }
            }
            if (grouping == null)
                return false;

            if (grouping.Remove(element))
            {
                if (grouping.Count == 0)
                {
                    groupings.RemoveAt(index);
                    GroupingPool<TKey, TElement>.Release(grouping);
                }
                return true;
            }
            return false;
        }
        public void Clear()
        {
            foreach (var grouping in groupings)
            {
                GroupingPool<TKey, TElement>.Release(grouping);
            }
            groupings.Clear();
        }
        public void Sort(bool descending = false)
        {
            if (descending)
            {
                groupings.Sort(keySortComparerDescending);
            }
            else
            {
                groupings.Sort(keySortComparer);
            }
        }
        public IEnumerator<Grouping<TKey, TElement>> GetEnumerator()
        {
            return ((IEnumerable<Grouping<TKey, TElement>>)groupings).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)groupings).GetEnumerator();
        }
        public int Count => groupings.Count;
        public Grouping<TKey, TElement>? this[TKey key]
        {
            get
            {
                foreach (var grouping in groupings)
                {
                    if (keyEqualityComparer.Equals(grouping.Key, key))
                    {
                        return grouping;
                    }
                }
                return null;
            }
        }
        private static readonly IComparer<Grouping<TKey, TElement>> keySortComparer = new GroupingSortComparer(false);
        private static readonly IComparer<Grouping<TKey, TElement>> keySortComparerDescending = new GroupingSortComparer(true);
        private static readonly EqualityComparer<TKey> keyEqualityComparer = EqualityComparer<TKey>.Default;
        private List<Grouping<TKey, TElement>> groupings = new List<Grouping<TKey, TElement>>();
        private class GroupingSortComparer : SortComparer<TKey, Grouping<TKey, TElement>>
        {
            public GroupingSortComparer(bool descending) : base(descending) { }
            public override TKey GetKey(Grouping<TKey, TElement> element)
            {
                return element.Key;
            }
        }
    }
}