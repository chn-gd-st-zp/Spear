using Spear.Inf.Core.Attr;
using Spear.Inf.Core.SettingsGeneric;

namespace Spear.MidM.Redis
{
    [DIModeForSettings("RedisSettings", typeof(RedisSettings))]
    public class RedisSettings : ISettings
    {
        /// <summary>
        /// 链接
        /// </summary>
        public virtual string Connection { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string InstanceName { get; set; }

        /// <summary>
        /// 缓存库,默认为0
        /// </summary>
        public int DefaultDatabase { get; set; }

        /// <summary>
        /// 默认超时时间（分钟）
        /// </summary>
        public int DefaultTimeOutMinutes { get; set; } = 1;
    }
}
