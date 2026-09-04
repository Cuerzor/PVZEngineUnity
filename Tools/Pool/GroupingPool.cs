#nullable enable

using System.Collections.Generic;
using UnityEngine.Pool;

namespace PVZEngine.Tools
{
    public static class GroupingPool<TKey, TElement>
    {
        public static Grouping<TKey, TElement> Get(TKey key)
        {
            var grouping = innerPool.Get();
            grouping.Key = key;
            return grouping;
        }
        public static void Release(Grouping<TKey, TElement> list)
        {
            innerPool.Release(list);
        }
        private static Grouping<TKey, TElement> CreateFunc() => new Grouping<TKey, TElement>();
        private static void GetFunc(Grouping<TKey, TElement> item) { }
        private static void ReleaseFunc(Grouping<TKey, TElement> item) => item.Clear();
        private static readonly ObjectPool<Grouping<TKey, TElement>> innerPool = new(CreateFunc, GetFunc, ReleaseFunc);
    }
}