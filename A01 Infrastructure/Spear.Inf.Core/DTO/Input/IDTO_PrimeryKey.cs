namespace Spear.Inf.Core.DTO
{
    /// <summary>
    /// 主键
    /// </summary>
    public class IDTO_PrimeryKey<TKey> : IDTO_Input
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual TKey PrimeryKey { get; set; }
    }
}
