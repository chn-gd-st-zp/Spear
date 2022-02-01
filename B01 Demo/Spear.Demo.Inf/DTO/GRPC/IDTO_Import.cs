using MessagePack;

using Spear.Inf.Core.DTO;
using Spear.Inf.Core.Tool;

namespace Spear.Demo.Inf.DTO
{
    [MessagePackObject(true)]
    public class IDTO_Import : IDTO_Input
    {
        public string FileBase64 { get; set; }

        public override bool Validation(out string errorMsg)
        {
            errorMsg = string.Empty;

            if (FileBase64.IsEmptyString())
            {
                errorMsg = "文件不能为空";
                return false;
            }

            return true;
        }
    }
}
