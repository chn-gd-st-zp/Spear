using System;
using System.Drawing;
using System.IO;
using System.Linq;

using Spear.Inf.Core;
using Spear.Inf.Core.AppEntrance;
using Spear.Inf.Core.Injection;
using Spear.Inf.Core.Tool;

namespace Spear.MidM.Attachment
{
    public static class AttachmentHandlerHelper
    {
        public static AttachmentOperationSetting GetOperation(string key)
        {
            var setting = ServiceContext.Resolve<AttachmentSettings>();
            return setting.Operations.Where(o => string.Compare(o.Key, key, true) == 0).SingleOrDefault();
        }

        public static Tuple<Enum_AttachmentResult, IHandler> GetHandler(string key)
        {
            var operation = GetOperation(key);

            var handler = ServiceContext.ResolveByKeyed<IHandler>(operation.Handler);
            if (handler == null)
                return new Tuple<Enum_AttachmentResult, IHandler>(Enum_AttachmentResult.HandlerNotFound, null);

            return new Tuple<Enum_AttachmentResult, IHandler>(Enum_AttachmentResult.Success, handler);
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
                if (string.Compare(fileExt, ext, true) == 0)
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

    public interface IHandler : ITransient
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="base64Data">
        /// StartWith => "data:ext;base64," or "data:type/ext;base64,"
        /// Sample => "data:png;base64," or "data:image/png;base64,"
        /// </param>
        /// <returns></returns>
        AttachmentResult Opration(string key, string base64Data);
    }

    public abstract class Handler_Base : IHandler
    {
        protected readonly AttachmentSettings Setting;
        protected Enum_AttachmentHandler EHandler = Enum_AttachmentHandler.None;

        public Handler_Base() { Setting = ServiceContext.Resolve<AttachmentSettings>(); }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="base64Data">
        /// StartWith => "data:ext;base64," or "data:type/ext;base64,"
        /// Sample => "data:png;base64," or "data:image/png;base64,"
        /// </param>
        /// <returns></returns>
        public AttachmentResult Opration(string key, string base64Data)
        {
            var result = new AttachmentResult();

            key = key.ToLower();

            var operation = AttachmentHandlerHelper.GetOperation(key);
            if (operation == null)
            {
                result.State = Enum_AttachmentResult.OperationNotFound;
                return result;
            }

            var desc = base64Data.Substring(0, base64Data.IndexOf(","));
            desc = desc.Replace("data:", "");
            desc = desc.Replace(";base64", "");

            var fileExt = desc.IndexOf("/") != -1 ? desc.Substring(desc.IndexOf("/") + 1) : desc;
            result = AttachmentHandlerHelper.VerifyExt(EHandler, fileExt);
            if (result.State != Enum_AttachmentResult.Success)
                return result;

            var basePath = AppInitHelper.GenericPath(Setting.Basic.PathMode, Setting.Basic.PathAddr) + key;
            var fileName = Unique.GetGUID();

            basePath = basePath.ToLower();
            fileName = fileName.ToLower();
            fileExt = fileExt.ToLower();

            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);

            var datas = base64Data.Substring(base64Data.IndexOf(",") + 1);

            result = AttachmentHandlerHelper.VerifySize(datas, operation.MaxKB);
            if (result.State != Enum_AttachmentResult.Success)
                return result;

            using (var stream = datas.Convert2Stream())
            {
                result = Do(operation, stream, basePath, fileName, fileExt);
                if (result.State == Enum_AttachmentResult.Success)
                {
                    Image oringnal = Image.FromStream(stream);
                    oringnal.Save($"{basePath}/{fileName}.{fileExt}");
                    result.FilePath = $"/{key}/{fileName}.{fileExt}";
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        protected abstract AttachmentResult Do(AttachmentOperationSetting operation, Stream stream, string path, string fileName, string fileExt);
    }
}
