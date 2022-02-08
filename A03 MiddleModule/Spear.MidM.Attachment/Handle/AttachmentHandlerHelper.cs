using System;
using System.Linq;

using Spear.Inf.Core;
using Spear.Inf.Core.Tool;

namespace Spear.MidM.Attachment
{
    public static class AttachmentHandlerHelper
    {
        public static Tuple<Enum_AttachmentResult, IHandler> GetHandler(string key)
        {
            var operation = GetOperation(key);

            var handler = ServiceContext.ResolveByKeyed<IHandler>(operation.Handler);
            if (handler == null)
                return new Tuple<Enum_AttachmentResult, IHandler>(Enum_AttachmentResult.HandlerNotFound, null);

            return new Tuple<Enum_AttachmentResult, IHandler>(Enum_AttachmentResult.Success, handler);
        }

        public static AttachmentOperationSetting GetOperation(string key)
        {
            var setting = ServiceContext.Resolve<AttachmentSettings>();
            return setting.Operations.Where(o => o.Key.IsEqual(key)).SingleOrDefault();
        }

        public static AttachmentResult VerifyExt(Enum_AttachmentHandler eHandler, string fileExt)
        {
            var result = new AttachmentResult();

            var setting = ServiceContext.Resolve<AttachmentSettings>();

            var handler = setting.Basic.Handlers.Where(o => o.Handler == eHandler).SingleOrDefault();
            if (handler == null)
            {
                result.State = Enum_AttachmentResult.HandlerNotFound;
                return result;
            }

            result.State = Enum_AttachmentResult.ExtNotSupport;

            foreach (var ext in handler.Exts)
            {
                if (fileExt.IsEqual(ext))
                {
                    result.State = Enum_AttachmentResult.Success;
                    return result;
                }
            }

            return result;
        }

        public static AttachmentResult VerifySize(string base64Data, int maxKB)
        {
            var result = new AttachmentResult();
            result.State = Enum_AttachmentResult.Success;

            if (base64Data.ToBytes().Length > maxKB * 1024)
            {
                result.State = Enum_AttachmentResult.OverSize;
                return result;
            }

            return result;
        }
    }
}
