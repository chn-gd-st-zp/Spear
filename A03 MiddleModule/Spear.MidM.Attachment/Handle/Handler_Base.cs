using System.Drawing;
using System.IO;

using Spear.Inf.Core;
using Spear.Inf.Core.Injection;
using Spear.Inf.Core.Tool;

namespace Spear.MidM.Attachment
{
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
        AttachmentResult Operation(string key, string base64Data);
    }

    public abstract class Handler_Base : IHandler
    {
        protected readonly AttachmentSettings Setting;

        protected abstract Enum_AttachmentHandler EHandler { get; }

        public Handler_Base() { Setting = ServiceContext.Resolve<AttachmentSettings>(); }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="base64Data">
        /// StartWith => "data:ext;base64," or "data:type/ext;base64,"
        /// Example => "data:png;base64," or "data:image/png;base64,"
        /// </param>
        /// <returns></returns>
        public AttachmentResult Operation(string key, string base64Data)
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
            desc = desc.Replace("data:", string.Empty);
            desc = desc.Replace(";base64", string.Empty);

            var fileExt = desc.IndexOf("/") != -1 ? desc.Substring(desc.IndexOf("/") + 1) : desc;
            result = AttachmentHandlerHelper.VerifyExt(EHandler, fileExt);
            if (result.State != Enum_AttachmentResult.Success)
                return result;

            var datas = base64Data.Substring(base64Data.IndexOf(",") + 1);
            result = AttachmentHandlerHelper.VerifySize(datas, operation.MaxKB);
            if (result.State != Enum_AttachmentResult.Success)
                return result;

            var basePath = AppInitHelper.GenericPath(Setting.Basic.PathMode, Setting.Basic.PathAddr) + "/" + key;
            var fileName = Unique.GetGUID();

            basePath = basePath.ToLower();
            fileName = fileName.ToLower();
            fileExt = fileExt.ToLower();

            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);

            using (var stream = datas.ToStream())
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
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <param name="fileExt"></param>
        /// <returns></returns>
        protected abstract AttachmentResult Do(AttachmentOperationSetting operation, Stream stream, string path, string fileName, string fileExt);
    }
}
