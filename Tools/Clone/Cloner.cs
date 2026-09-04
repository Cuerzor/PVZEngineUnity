#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;

namespace PVZEngine.Tools.Cloning
{
    public static class Cloner
    {
        public static object? CloneValue(object? original)
        {
            if (original == null)
                return null;

            // 1. 值类型和字符串直接返回
            Type type = original.GetType();
            if (type.IsValueType || original is string)
                return original;

            // 2. 优先检查注册表（允许外部覆盖任何类型）
            if (CloneRegistry.TryGetCloner(type, out var cloner))
                return cloner.Clone(original);

            // 3. 检查是否实现了 ICopyable 接口
            if (original is ICanClone copyable)
                return copyable.Clone();

            return DefaultClone(type, original);
        }
        public static object? DefaultClone(Type type, object original)
        {
            // 4. 精确处理数组（T[]）
            if (type.IsArray)
            {
                return ArrayClone(type, original);
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                return ListGenericClone(type, original);
            }

            // 4. 实在不知道如何复制，抛出明确异常提醒注册
            throw new NotSupportedException($"Unregistered type to clone: {original.GetType()}");
        }
        public static object? ArrayClone(Type type, object original)
        {
            Array sourceArray = (Array)original;
            Type elementType = type.GetElementType();
            Array newArray = Array.CreateInstance(elementType, sourceArray.Length);
            for (int i = 0; i < sourceArray.Length; i++)
            {
                newArray.SetValue(CloneValue(sourceArray.GetValue(i)), i);
            }
            return newArray;
        }
        public static object? ListGenericClone(Type type, object original)
        {
            Type itemType = type.GetGenericArguments()[0];
            // 使用 Activator 创建同类型的新 List<T>
            var newList = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(itemType));

            var sourceList = (IList)original;
            foreach (var item in sourceList)
            {
                newList.Add(CloneValue(item));
            }
            return newList;
        }
    }
    public static class CloneRegistry
    {
        public static void Register<T>(IObjectCloner cloner)
        {
            cloners[typeof(T)] = cloner;
        }
        public static bool TryGetCloner(Type type, out IObjectCloner cloner)
        {
            return cloners.TryGetValue(type, out cloner);
        }

        // 存储克隆委托，为了性能使用 Func<object, object>
        private static readonly Dictionary<Type, IObjectCloner> cloners = new();
    }
    public interface IObjectCloner
    {
        object? Clone(object? original);
    }
    public interface ICanClone
    {
        object Clone();
    }
}