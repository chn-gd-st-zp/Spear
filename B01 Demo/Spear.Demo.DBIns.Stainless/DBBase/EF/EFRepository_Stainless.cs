using Spear.Inf.Core.DBRef;
using Spear.Inf.EF;

namespace Spear.Demo.DBIns.Stainless
{
    public class EFRepository_Stainless<TEntity, TKey> : EFRepository<EFDBContext_Stainless, TEntity, TKey>
        where TEntity : class, IDBEntity, IDBField_PrimeryKey<TKey>, new()
    {
        //
    }
}
