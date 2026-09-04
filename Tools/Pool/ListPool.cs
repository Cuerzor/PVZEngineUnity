#nullable enable

using System.Collections.Generic;
using PVZEngine.Tools.Unity;
using UnityEngine.Pool;

namespace PVZEngine.Tools
{
    public static class RentPool
    {
        public static PoolItem<T2> Rent<T2>(ObjectPool<T2> innerPool) where T2 : class
        {
            // 检查当前池大小（假设 ObjectPool 有 Count 属性）
            int currentSize = innerPool.CountActive; // 若没有，则需自己维护计数器
            if (currentSize > _maxAllowedSize)
            {
                Log.LogWarning($"{typeof(T2).Name} 池大小 ({currentSize}) 超过阈值 ({_maxAllowedSize})，可能存在内存泄漏！");
            }

            return new PoolItem<T2>(innerPool);
        }
        private const int _maxAllowedSize = 20;
    }
    public static class ListPool<T>
    {
        public static PoolItem<List<T>> Rent()
        {
            return RentPool.Rent<List<T>>(innerPool);
        }
        private static List<T> CreateFunc() => new List<T>();
        private static void GetFunc(List<T> item) { }
        private static void ReleaseFunc(List<T> item) => item.Clear();
        private static readonly ObjectPool<List<T>> innerPool = new(CreateFunc, GetFunc, ReleaseFunc);
    }
}