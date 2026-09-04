#nullable enable

using System;
using UnityEngine.Pool;

namespace PVZEngine.Tools
{
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