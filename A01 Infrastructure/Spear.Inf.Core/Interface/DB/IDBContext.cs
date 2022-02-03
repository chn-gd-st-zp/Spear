using System;

using Spear.Inf.Core.DBRef;

namespace Spear.Inf.Core.Interface
{
    public interface IDBContext : IDBFunc4DBContext
    {
        string ID { get; }

        int SaveChanges();

        string GetTBName(Type dbType);

        string GetTBName<TEntity>() where TEntity : DBEntity_Base, new();

        string GetPKName(Type dbType);

        string GetPKName<TEntity>() where TEntity : DBEntity_Base, new();

        object GetQueryable<TEntity>() where TEntity : DBEntity_Base, new();
    }
}
