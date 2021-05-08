using Spear.Inf.Core.Attr;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.SettingsGeneric;

namespace Spear.MidM.SessionNAuth
{
    [DIModeForSettings("SessionNAuthSettings", Enum_DIType.Specific, typeof(SessionNAuthSettings))]
    public class SessionNAuthSettings : ISettings
    {
        /// <summary>
        /// AccessToken在Header中的标识
        /// </summary>
        public string AccessTokenKeyInHeader { get; set; }

        /// <summary>
        /// 缓存DBIndex
        /// </summary>
        public int CacheDBIndex { get; set; }

        /// <summary>
        /// 缓存前缀
        /// </summary>
        public string CachePrefix { get; set; }

        /// <summary>
        /// 缓存有效持续时间（分钟）
        /// </summary>
        public int CacheValidDuration { get; set; }
    }
}
