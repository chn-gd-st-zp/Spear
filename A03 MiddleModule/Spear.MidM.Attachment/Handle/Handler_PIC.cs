using System.Drawing;
using System.IO;

using Spear.Inf.Core.Attr;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.DataFile;
using Spear.Inf.Core.Tool;

namespace Spear.MidM.Attachment
{
    [DIModeForService(Enum_DIType.ExclusiveByKeyed, typeof(IHandler), Enum_AttachmentHandler.PIC)]
    public class Handler_PIC : Handler_Base
    {
        protected override Enum_AttachmentHandler EHandler { get { return Enum_AttachmentHandler.PIC; } }

        protected override AttachmentResult Do(AttachmentOperationSetting operation, Stream stream, string path, string fileName, string fileExt)
        {
            var result = new AttachmentResult();

            var thumbnail = new Thumbnail(stream);

            foreach (var arg in operation.ParseArgs<AttachmentPictureOperationArgSettings>())
            {
                Image image = null;

                if (!arg.ShrinkTo.IsEmptyString())
                    image = thumbnail.Draw(int.Parse(arg.ShrinkTo) * 1.0 / 100);
                else if (!arg.Width.IsEmptyString() && !arg.Width.IsEmptyString())
                    image = thumbnail.Draw(int.Parse(arg.Width), int.Parse(arg.Height));

                if (image == null)
                    continue;

                image.Save($"{path}/{fileName}_{arg.Suffix}.{fileExt}");
                image.Dispose();
            }

            result.State = Enum_AttachmentResult.Success;
            return result;
        }
    }
}
