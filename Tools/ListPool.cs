#nullable enable

using System;
using System.Collections.Generic;
using UnityEngine.Pool;

namespace PVZEngine.Tools
{
    public static class ListPool<T>
    {
        public static PoolItem<List<T>> Rent()
        {
            return new PoolItem<List<T>>(innerPool);
        }
        private static List<T> CreateFunc() => new List<T>();
        private static void GetFunc(List<T> item) { }
        private static void ReleaseFunc(List<T> item) => item.Clear();
        private static readonly ObjectPool<List<T>> innerPool = new(CreateFunc, GetFunc, ReleaseFunc);
    }
    public static class HashSetPool<T>
    {
        public static PoolItem<HashSet<T>> Rent()
        {
            return new PoolItem<HashSet<T>>(innerPool);
        }
        private static HashSet<T> CreateFunc() => new HashSet<T>();
        private static void GetFunc(HashSet<T> item) { }
        private static void ReleaseFunc(HashSet<T> item) => item.Clear();
        private static readonly ObjectPool<HashSet<T>> innerPool = new(CreateFunc, GetFunc, ReleaseFunc);
    }
    public struct PoolItem<T> : IDisposable where T : class
    {
        public PoolItem(ObjectPool<T> pool)
        {
            this.pool = pool;
            Value = pool.Get();
        }
        public void Dispose()
        {
            if (pool != null && Value != null)
            {
                pool.Release(Value);
                pool = null;
                Value = null!;
            }
        }
        public T Value { get; private set; }
        private ObjectPool<T>? pool;
    }
}