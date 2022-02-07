using Spear.Inf.Core;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.DBRef;
using Spear.Inf.Core.Interface;

namespace Spear.Inf.EF
{
    public class EFRepository : DBRepository
    {
        protected override IDBContext GetDBContext() { return ServiceContext.ResolveByKeyed<IDBContext>(Enum_ORMType.EF); }
    }

    public class EFRepository<TEntity> : DBRepository<TEntity>
        where TEntity : class, IDBEntity, new()
    {
        protected override IDBContext GetDBContext() { return ServiceContext.ResolveByKeyed<IDBContext>(Enum_ORMType.EF); }
    }

    public class EFRepository<TEntity, TKey> : DBRepository<TEntity, TKey>
        where TEntity : class, IDBEntity, IDBField_PrimeryKey<TKey>, new()
    {
        protected override IDBContext GetDBContext() { return ServiceContext.ResolveByKeyed<IDBContext>(Enum_ORMType.EF); }
    }

    public class EFRepository<TDBContext, TEntity, TKey> : DBRepository<TDBContext, TEntity, TKey>
        where TDBContext : EFDBContext, IDBContext
        where TEntity : class, IDBEntity, IDBField_PrimeryKey<TKey>, new()
    {
        protected override IDBContext GetDBContext() { return ServiceContext.ResolveByKeyed<TDBContext>(Enum_ORMType.EF); }
    }
}
