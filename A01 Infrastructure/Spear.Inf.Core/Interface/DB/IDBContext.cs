using System;

using Spear.Inf.Core.DBRef;

namespace Spear.Inf.Core.Interface
{
    public interface IDBContext : IDBFunc4DBContext
    {
        string ID { get; }

        string GetTBName(Type dbType);

        string GetTBName<TEntity>() where TEntity : class, IDBEntity, new();

        string GetPKName(Type dbType);

        string GetPKName<TEntity>() where TEntity : class, IDBEntity, new();

        int SaveChanges();

        object GetQueryable<TEntity>() where TEntity : class, IDBEntity, new();
    }
}
