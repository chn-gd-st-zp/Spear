using Spear.Inf.Core.Attr;
using Spear.Inf.Core.SettingsGeneric;

namespace Spear.MidM.RabbitMQ
{
    [DIModeForSettings("RabbitMQSettings", typeof(RabbitMQSettings))]
    public class RabbitMQSettings : ISettings
    {
        /// <summary>
        /// 主机名
        /// </summary>
        public virtual string HostName { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public virtual string Port { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public virtual string Password { get; set; }

        /// <summary>
        /// 虚拟主机
        /// </summary>
        public string VirtualHost { get; set; }

        /// <summary>
        /// 心跳检测（秒）
        /// </summary>
        public int Heartbeat { get; set; }
    }
}
