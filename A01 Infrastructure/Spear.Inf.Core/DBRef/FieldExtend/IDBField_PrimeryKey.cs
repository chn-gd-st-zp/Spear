using System.Reflection;

namespace Spear.Inf.Core.DBRef
{
    public interface IDBField_PrimeryKey
    {
        //
    }

    public interface IDBField_PrimeryKey<T> : IDBField_PrimeryKey
    {
        T PrimeryKey { get; set; }
    }

    public static class IDBField_PrimeryKey_Extend
    {
        public static PropertyInfo GetPKPropertyInfo(this IDBField_PrimeryKey obj)
        {
            return obj.GetType().GetProperty("PrimeryKey");
        }
    }
}
