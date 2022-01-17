using MessagePack;

using Spear.Inf.Core.DTO;
using Spear.Inf.Core.Tool;

namespace Spear.Demo4GRPC.Pub.TestDemo
{
    [MessagePackObject(true)]
    public class ImportParam : IDTO_Input
    {
        public string FileBase64 { get; set; }

        public override bool VerifyField(out string errorMsg)
        {
            errorMsg = "";

            if (FileBase64.IsEmptyString())
            {
                errorMsg = "文件不能为空";
                return false;
            }

            return true;
        }
    }
}
