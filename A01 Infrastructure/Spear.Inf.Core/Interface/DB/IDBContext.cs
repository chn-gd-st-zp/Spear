using Spear.Inf.Core.DBRef;

namespace Spear.Inf.Core.Interface
{
    public interface IDBContext : IDBFunc4DBContext
    {
        string ID { get; }

        int SaveChanges();

        object GetQueryable<TEntity>() where TEntity : DBEntity_Base, new();
    }
}
