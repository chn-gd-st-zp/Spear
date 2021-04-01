using Spear.Inf.Core.DBRef;

namespace Spear.Inf.Core.Interface
{
    public interface IDBRepository : IDBFunc4Repository
    {
        IDBContext DBContext { get; }
    }

    public interface IDBRepository<TEntity> : IDBFunc4Repository<TEntity>, IDBRepository
        where TEntity : DBEntity_Basic, new()
    {
        //
    }

    public interface IDBRepository<TEntity, TKey> : IDBFunc4Repository<TEntity, TKey>, IDBRepository<TEntity>
        where TEntity : DBEntity_Basic, new()
    {
        //
    }

    public interface IDBRepository<TDBContext, TEntity, TKey> : IDBRepository<TEntity, TKey>
        where TDBContext : IDBContext
        where TEntity : DBEntity_Basic, new()
    {
        new TDBContext DBContext { get; }
    }
}
