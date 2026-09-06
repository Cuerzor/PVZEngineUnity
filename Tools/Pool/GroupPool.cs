#nullable enable

using UnityEngine.Pool;

namespace PVZEngine.Tools
{
    public static class GroupPool<TKey, TElement>
    {
        public static PoolItem<Group<TKey, TElement>> Rent()
        {
            return RentPool.Rent<Group<TKey, TElement>>(innerPool);
        }
        private static Group<TKey, TElement> CreateFunc() => new Group<TKey, TElement>();
        private static void GetFunc(Group<TKey, TElement> item) { }
        private static void ReleaseFunc(Group<TKey, TElement> item) => item.Clear();
        private static readonly ObjectPool<Group<TKey, TElement>> innerPool = new(CreateFunc, GetFunc, ReleaseFunc);
    }
}