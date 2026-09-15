#nullable enable

using System;
using System.Collections.Generic;

namespace PVZEngine.Modifiers
{
    public class ModifierLibrary
    {
        public ModifierLibrary()
        {
        }
        #region 属性
        public IEnumerable<IPropertyKey> GetModifyPropertyKeys()
        {
            return modifierCachesForProperty.Keys;
        }
        public void GetModifierItemsForProperty(IPropertyKey name, List<ModifierSourceItem> results)
        {
            if (!modifierCachesForProperty.TryGetValue(name, out var list))
                return;
            noStackModifierBuffer.Clear();
            foreach (var element in list)
            {
                var modifier = element.modifier;
                if (modifier.NoStack)
                {
                    if (noStackModifierBuffer.Contains(modifier))
                    {
                        continue;
                    }
                    else
                    {
                        noStackModifierBuffer.Add(modifier);
                    }
                }
                results.Add(element);
            }
        }
        #endregion

        #region 修改器缓存
        public void AddModifierCache(ModifierSourceItem item, bool serialization)
        {
            var modifier = item.modifier;
            var modifyName = modifier.PropertyName;
            var usingName = modifier.UsingContainerPropertyName;
            if (!modifierCachesForProperty.TryGetValue(modifyName, out var list))
            {
                list = new List<ModifierSourceItem>();
                modifierCachesForProperty.Add(modifyName, list);
            }
            list.Add(item);

            if (PropertyKeyHelper.IsValid(usingName))
            {
                if (!modifierCachesUsingProperty.TryGetValue(usingName, out var usingList))
                {
                    usingList = new List<ModifierSourceItem>();
                    modifierCachesUsingProperty.Add(usingName, usingList);
                }
                usingList.Add(item);
            }

            CallModifiedPropertyChanged(modifyName, serialization);
        }
        public void RemoveModifierCache(ModifierSourceItem item, bool serialization)
        {
            var modifier = item.modifier;
            var modifyName = modifier.PropertyName;
            if (modifierCachesForProperty.TryGetValue(modifyName, out var list))
            {
                list.Remove(item);
            }

            var usingName = modifier.UsingContainerPropertyName;
            if (PropertyKeyHelper.IsValid(usingName))
            {
                if (modifierCachesUsingProperty.TryGetValue(usingName, out var usingList))
                {
                    usingList.Remove(item);
                }
            }

            CallModifiedPropertyChanged(modifyName, serialization);
        }
        public void ClearModifierCaches(bool serialization)
        {
            foreach (var pair in modifierCachesForProperty)
            {
                var modifyName = pair.Key;
                if (modifierCachesForProperty.TryGetValue(modifyName, out var list))
                {
                    list.Clear();
                }
                CallModifiedPropertyChanged(modifyName, serialization);
            }
            foreach (var pair in modifierCachesUsingProperty)
            {
                var usingName = pair.Key;
                if (modifierCachesUsingProperty.TryGetValue(usingName, out var list))
                {
                    list.Clear();
                }
            }
            modifierCachesForProperty.Clear();
            modifierCachesUsingProperty.Clear();
        }
        #endregion

        public void CallPropertyChanged(IModifierSource source, IPropertyKey key)
        {
            if (!modifierCachesUsingProperty.TryGetValue(key, out var list))
                return;
            foreach (var item in list)
            {
                if (item.container != source)
                    continue;
                var modifier = item.modifier;
                if (key.Equals(modifier.UsingContainerPropertyName))
                {
                    CallModifiedPropertyChanged(modifier.PropertyName, false);
                }
            }
        }
        public void CallModifiedPropertyChanged(IPropertyKey key, bool serialization)
        {
            OnModifiedPropertyNeedsUpdate?.Invoke(key, serialization);
        }
        public event Action<IPropertyKey, bool>? OnModifiedPropertyNeedsUpdate;

        #region 属性字段
        private Dictionary<IPropertyKey, List<ModifierSourceItem>> modifierCachesForProperty = new Dictionary<IPropertyKey, List<ModifierSourceItem>>(new PropertyKeyComparer());
        private Dictionary<IPropertyKey, List<ModifierSourceItem>> modifierCachesUsingProperty = new Dictionary<IPropertyKey, List<ModifierSourceItem>>(new PropertyKeyComparer());
        private HashSet<PropertyModifier> noStackModifierBuffer = new HashSet<PropertyModifier>();
        #endregion
    }
}