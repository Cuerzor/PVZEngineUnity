using System.Collections.Generic;

namespace PVZEngine.Tools
{
    public abstract class SortComparer<TKey, TElement> : IComparer<TElement>
    {
        public SortComparer(bool descending)
        {
            this.descending = descending;
        }
        public int Compare(TElement x, TElement y)
        {
            var xKey = GetKey(x);
            var yKey = GetKey(y);
            if (descending)
            {
                return comparer.Compare(yKey, xKey);
            }
            return comparer.Compare(xKey, yKey);
        }
        public abstract TKey GetKey(TElement element);
        private static readonly Comparer<TKey> comparer = Comparer<TKey>.Default;
        private bool descending = false;
    }
}