using MessagePack;

using Spear.Inf.Core.Attr;

namespace Spear.Inf.Core.DTO
{
    [MessagePackObject(true)]
    public class IDTO_Range<T> : IDTO
    {
        [Remark("开始")]
        public T Begin { get; set; }

        [Remark("结束")]
        public T End { get; set; }
    }
}
