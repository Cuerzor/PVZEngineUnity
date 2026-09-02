#nullable enable
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace PVZEngine.Tools
{
    public static class WeakLookupCache<TKey, TValue> where TKey : class where TValue : class
    {
        public static TValue GetOrCreate(TKey key, ConditionalWeakTable<TKey, TValue>.CreateValueCallback valueFactory)
        {
            return _cache.GetValue(key, valueFactory);
        }

        private static readonly ConditionalWeakTable<TKey, TValue> _cache = new ConditionalWeakTable<TKey, TValue>();
    }
}