using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.DBRef;
using Spear.Inf.Core.Interface;
using Spear.Inf.Core.ServGeneric;

namespace Spear.Inf.EF
{
    public class EFRepository : DBRepository
    {
        protected override IDBContext GetDBContext() { return ServiceContext.ResolveByKeyed<IDBContext>(Enum_DBType.EF); }
    }

    public class EFRepository<TEntity> : DBRepository<TEntity>
        where TEntity : DBEntity_Basic, new()
    {
        protected override IDBContext GetDBContext() { return ServiceContext.ResolveByKeyed<IDBContext>(Enum_DBType.EF); }
    }

    public class EFRepository<TEntity, TKey> : DBRepository<TEntity, TKey>
        where TEntity : DBEntity_Basic, IDBField_ID<TKey>, new()
    {
        protected override IDBContext GetDBContext() { return ServiceContext.ResolveByKeyed<IDBContext>(Enum_DBType.EF); }
    }

    public class EFRepository<TDBContext, TEntity, TKey> : DBRepository<TDBContext, TEntity, TKey>
        where TDBContext : EFDBContext, IDBContext
        where TEntity : DBEntity_Basic, IDBField_ID<TKey>, new()
    {
        protected override IDBContext GetDBContext() { return ServiceContext.ResolveByKeyed<TDBContext>(Enum_DBType.EF); }
    }
}
