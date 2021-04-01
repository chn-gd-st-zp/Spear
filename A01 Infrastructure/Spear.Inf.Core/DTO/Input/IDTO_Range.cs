using MessagePack;

namespace Spear.Inf.Core.DTO
{
    [MessagePackObject(true)]
    public class IDTO_Range<T>
    {
        public T Begin { get; set; }

        public T End { get; set; }
    }
}
