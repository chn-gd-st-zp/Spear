using MessagePack;

using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.DTO;

namespace Spear.Demo.Inf.DTO
{
    [MessagePackObject(true)]
    public class IDTO_Export : IDTO_List
    {
        public Enum_Status EStatus { get; set; }

        public override bool Validation(out string errorMsg)
        {
            errorMsg = string.Empty;

            if (EStatus == Enum_Status.None)
            {
                errorMsg = "状态不能为空";
                return false;
            }

            return true;
        }
    }
}
