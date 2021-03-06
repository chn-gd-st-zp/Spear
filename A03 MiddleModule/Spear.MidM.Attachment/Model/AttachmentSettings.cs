using System;
using System.Collections.Generic;
using System.Linq;

using Spear.Inf.Core.Attr;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.SettingsGeneric;
using Spear.Inf.Core.Tool;

namespace Spear.MidM.Attachment
{
    [DIModeForSettings("AttachmentSettings", typeof(AttachmentSettings))]
    public class AttachmentSettings : ISettings
    {
        public AttachmentBasicSetting Basic { get; set; }

        public List<AttachmentOperationSetting> Operations { get; set; }
    }

    public class AttachmentBasicSetting
    {
        public Enum_PathMode PathMode { get; set; }

        public string PathAddr { get; set; }

        public List<AttachmentHandlerSetting> Handlers { get; set; }
    }

    public class AttachmentHandlerSetting
    {
        public Enum_AttachmentHandler Handler { get; set; }

        public string[] Exts { get; set; }
    }

    public class AttachmentOperationSetting
    {
        public Enum_AttachmentHandler Handler { get; set; }

        public string Key { get; set; }

        public int MaxKB { get; set; }

        public string[] Args { get; set; }

        public List<TSettings> ParseArgs<TSettings>() where TSettings : class, AttachmentOperationArgSettings, new()
        {
            List<TSettings> result = new List<TSettings>();

            var props = typeof(TSettings).GetProperties();

            if (Args == null || Args.Count() == 0)
                return result;

            foreach (var arg in Args)
            {
                TSettings resultItem = new TSettings();

                var paramArray = arg.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var param in paramArray)
                {
                    var datas = param.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    var key = datas[0];
                    var value = datas[1];

                    var prop = props.Where(o => o.Name == key).SingleOrDefault();
                    if (prop == null)
                        continue;

                    prop.SetValue(resultItem, value);
                }

                result.Add(resultItem);
            }

            return result;
        }
    }

    public interface AttachmentOperationArgSettings { }

    public class AttachmentPictureOperationArgSettings : AttachmentOperationArgSettings
    {
        public string Type { get; set; }

        public string Suffix { get; set; }

        public string ShrinkTo { get; set; }

        public string Width { get; set; }

        public string Height { get; set; }

        public Enum_AttachmentPictureSize EType { get { return Type.ToEnum<Enum_AttachmentPictureSize>(); } }
    }
}
