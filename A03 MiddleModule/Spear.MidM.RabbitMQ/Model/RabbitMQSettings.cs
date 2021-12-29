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
        //[Decrypt(Enum_EncryptionNDecrypt.AES, Enum_Environment.Staging, Enum_Environment.Production)]
        public string HostName { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        //[Decrypt(Enum_EncryptionNDecrypt.AES, Enum_Environment.Staging, Enum_Environment.Production)]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        //[Decrypt(Enum_EncryptionNDecrypt.AES, Enum_Environment.Staging, Enum_Environment.Production)]
        public string Password { get; set; }

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
