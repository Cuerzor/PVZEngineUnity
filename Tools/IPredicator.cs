#nullable enable
using System.Runtime.CompilerServices;

namespace PVZEngine.Tools
{
    public interface IPredicator<T>
    {
        public bool IsMatch(T value);
    }
}