﻿using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.DBRef;
using Spear.Inf.Core.Interface;
using Spear.Inf.Core.ServGeneric;

namespace Spear.Inf.SqlSugar
{
    public class SSRepository : DBRepository
    {
        protected override IDBContext GetDBContext()
        {
            return ServiceContext.ResolveServByKeyed<IDBContext>(Enum_DBType.SqlSugar);
        }
    }

    public class SSRepository<TEntity> : DBRepository<TEntity>
        where TEntity : DBEntity_Basic, new()
    {
        protected override IDBContext GetDBContext()
        {
            return ServiceContext.ResolveServByKeyed<IDBContext>(Enum_DBType.SqlSugar);
        }
    }

    public class SSRepository<TEntity, TKey> : DBRepository<TEntity, TKey>
        where TEntity : DBEntity_Basic, new()
    {
        protected override IDBContext GetDBContext()
        {
            return ServiceContext.ResolveServByKeyed<IDBContext>(Enum_DBType.SqlSugar);
        }
    }

    public class SSRepository<TDBContext, TEntity, TKey> : DBRepository<TDBContext, TEntity, TKey>
        where TDBContext : SSDBContext, IDBContext
        where TEntity : DBEntity_Basic, new()
    {
        protected override IDBContext GetDBContext()
        {
            return ServiceContext.ResolveServByKeyed<TDBContext>(Enum_DBType.SqlSugar);
        }
    }
}
