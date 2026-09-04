#nullable enable

using System;
using UnityEngine.Pool;

namespace PVZEngine.Tools
{
    public readonly struct PoolItem<T> : IDisposable where T : class
    {
        public PoolItem(ObjectPool<T> pool)
        {
            this.pool = pool;
            value = pool.Get();
        }
        public void Dispose()
        {
            if (pool != null && Value != null)
            {
                pool.Release(Value);
            }
        }
        public T Value => value;
        private readonly T value;
        private readonly ObjectPool<T>? pool;
    }
}