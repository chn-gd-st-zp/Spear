using Spear.Inf.Core;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.DBRef;
using Spear.Inf.Core.Interface;

namespace Spear.Inf.SqlSugar
{
    public class SSRepository : DBRepository
    {
        protected override IDBContext GetDBContext() { return ServiceContext.ResolveByKeyed<IDBContext>(Enum_ORMType.SqlSugar); }
    }

    public class SSRepository<TEntity> : DBRepository<TEntity>
        where TEntity : class, IDBEntity, new()
    {
        protected override IDBContext GetDBContext() { return ServiceContext.ResolveByKeyed<IDBContext>(Enum_ORMType.SqlSugar); }
    }

    public class SSRepository<TEntity, TKey> : DBRepository<TEntity, TKey>
        where TEntity : class, IDBEntity, IDBField_PrimeryKey<TKey>, new()
    {
        protected override IDBContext GetDBContext() { return ServiceContext.ResolveByKeyed<IDBContext>(Enum_ORMType.SqlSugar); }
    }

    public class SSRepository<TDBContext, TEntity, TKey> : DBRepository<TDBContext, TEntity, TKey>
        where TDBContext : SSDBContext, IDBContext
        where TEntity : class, IDBEntity, IDBField_PrimeryKey<TKey>, new()
    {
        protected override IDBContext GetDBContext() { return ServiceContext.ResolveByKeyed<TDBContext>(Enum_ORMType.SqlSugar); }
    }
}
