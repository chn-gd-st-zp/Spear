using System.IO;

using Spear.Inf.Core.Attr;
using Spear.Inf.Core.CusEnum;

namespace Spear.MidM.Attachment
{
    [DIModeForService(Enum_DIType.ExclusiveByKeyed, typeof(IHandler), Enum_AttachmentHandler.DOC)]
    public class Handler_DOC : Handler_Base
    {
        protected override Enum_AttachmentHandler EHandler { get { return Enum_AttachmentHandler.DOC; } }

        protected override AttachmentResult Do(AttachmentOperationSetting operation, Stream stream, string path, string fileName, string fileExt)
        {
            var result = new AttachmentResult();

            #region operation

            //

            #endregion

            result.State = Enum_AttachmentResult.Success;
            return result;
        }
    }
}
