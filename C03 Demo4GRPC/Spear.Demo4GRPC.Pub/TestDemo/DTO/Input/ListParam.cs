using MessagePack;

using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.DTO;

namespace Spear.Demo4GRPC.Pub.TestDemo
{
    [MessagePackObject(true)]
    public class ListParam : IDTO_List
    {
        public Enum_Status EStatus { get; set; }

        public override bool VerifyField(out string errorMsg)
        {
            errorMsg = "";

            if (EStatus == Enum_Status.None)
            {
                errorMsg = "状态不能为空";
                return false;
            }

            return true;
        }
    }
}
