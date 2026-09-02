#nullable enable

using PVZEngine.Level;
using System.Linq;

namespace PVZEngine.Auras
{
    /// <summary>
    /// 光环组件，封装 Create/Update/Serialize 的完整生命周期。
    /// 用于 Entity、Armor、Buff 等实现了 IAuraSource 的类型。
    /// </summary>
    public struct AuraComponent
    {
        public AuraEffectList List { get; private set; }

        public void Init(IAuraSource source, AuraEffectDefinition[] definitions)
        {
            List = new AuraEffectList();
            var level = source.GetLevel();
            for (int i = 0; i < definitions.Length; i++)
            {
                List.Add(level, new AuraEffect(definitions[i], i, source));
            }
        }

        public void Update() => List?.Update();

        public void CloneTo(AuraComponent target)
        {
            if (List != null)
            {
                List.CloneTo(target.List);
            }
        }
        public void WriteToSerializable<T>(T seri) where T : IHasSerializableAuras
        {
            if (List != null)
                seri.SetSerializableAuras(List.GetAll().Select(a => a.ToSerializable()).ToArray());
        }

        public void LoadFromSerializable<T>(T seri, LevelEngine level) where T : IHasSerializableAuras
        {
            List?.LoadFromSerializable(level, seri.GetSerializableAuras());
        }
    }

    /// <summary>
    /// 可序列化光环持有者的接口，用于解耦序列化格式。
    /// </summary>
    public interface IHasSerializableAuras
    {
        SerializableAuraEffect?[]? GetSerializableAuras();
        void SetSerializableAuras(SerializableAuraEffect?[]? auras);
    }
}
