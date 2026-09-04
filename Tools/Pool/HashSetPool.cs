#nullable enable

using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine.Pool;

namespace PVZEngine.Tools
{
    public static class HashSetPool<T>
    {
        public static PoolItem<HashSet<T>> Rent()
        {
            return RentPool.Rent<HashSet<T>>(innerPool);
        }
        private static HashSet<T> CreateFunc() => new HashSet<T>();
        private static void GetFunc(HashSet<T> item) { }
        private static void ReleaseFunc(HashSet<T> item) => item.Clear();
        private static readonly ObjectPool<HashSet<T>> innerPool = new(CreateFunc, GetFunc, ReleaseFunc);
    }
}