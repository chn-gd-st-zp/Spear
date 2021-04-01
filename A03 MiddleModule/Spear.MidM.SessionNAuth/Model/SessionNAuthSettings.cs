using Spear.Inf.Core.Attr;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.SettingsGeneric;

namespace Spear.MidM.SessionNAuth
{
    [DIModeForSettings("SessionNAuthSettings", Enum_DIType.Specific, typeof(SessionNAuthSettings))]
    public class SessionNAuthSettings : ISettings
    {
        /// <summary>
        /// 在Header中的标识
        /// </summary>
        public string TokenKey { get; set; }

        /// <summary>
        /// 缓存前缀
        /// </summary>
        public string CachePrefix { get; set; }

        /// <summary>
        /// 有效持续时间（分钟）
        /// </summary>
        public int ValidDuration { get; set; }
    }
}
