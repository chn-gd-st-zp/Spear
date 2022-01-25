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
}
