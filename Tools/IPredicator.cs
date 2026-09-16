#nullable enable
namespace PVZEngine.Tools
{
    public interface IPredicator<T>
    {
        public bool IsMatch(T value);
    }
}