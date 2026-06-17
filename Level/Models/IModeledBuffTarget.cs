#nullable enable

using PVZEngine.Buffs;

namespace PVZEngine.Models
{
    public interface IModeledBuffTarget : IHasModel, IBuffTarget
    {
        IModelInterface? GetInsertedModel(NamespaceID key) => this.GetChildModel(key);
    }
}