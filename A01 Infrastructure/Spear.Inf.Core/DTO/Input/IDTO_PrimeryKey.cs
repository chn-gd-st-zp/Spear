using Spear.Inf.Core.Tool;

namespace Spear.Inf.Core.DTO
{
    /// <summary>
    /// 主键
    /// </summary>
    public interface IIDTO_PrimeryKey<TKey>
    {
        /// <summary>
        /// 主键
        /// </summary>
        TKey PrimeryKey { get; set; }
    }

    /// <summary>
    /// 主键
    /// </summary>
    public class IDTO_PrimeryKey<TKey> : IDTO_Input, IIDTO_PrimeryKey<TKey>
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual TKey PrimeryKey { get; set; }
    }

    public static class IDTO_PrimeryKey_Extend
    {
        public static object GetPrimeryKey(this object obj)
        {
            var type = obj.GetType();

            if (!type.IsGenericOf(typeof(IIDTO_PrimeryKey<>)))
                return null;

            var field = type.GetField("PrimeryKey");
            if (field == null)
                return null;

            return field.GetValue(obj);
        }
    }
}
