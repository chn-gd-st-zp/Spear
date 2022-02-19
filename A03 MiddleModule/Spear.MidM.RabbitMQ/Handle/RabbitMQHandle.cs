using System;

using RabbitMQ.Client;

using Spear.Inf.Core;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.Interface;

namespace Spear.MidM.RabbitMQ
{
    public abstract class RabbitMQHandle<TParam> : IMsgQueueHandle<TParam>
        where TParam : class, IMsgQueueParam
    {
        /// <summary>
        /// 处理器状态
        /// </summary>
        public Enum_Process EStatus { get; set; } = Enum_Process.None;

        /// <summary>
        /// 配置
        /// </summary>
        private readonly RabbitMQSettings config;

        /// <summary>
        /// 服务连接构造工厂
        /// </summary>
        private IConnectionFactory connectionFactory;

        /// <summary>
        /// 服务连接
        /// </summary>
        private IConnection connection;

        /// <summary>
        /// 服务通道
        /// </summary>
        public IModel Channel { get; private set; }

        /// <summary>
        /// 处理器初始化
        /// </summary>
        public RabbitMQHandle()
        {
            config = ServiceContext.Resolve<RabbitMQSettings>();

            Init();

            EStatus = Enum_Process.Processing;
        }

        /// <summary>
        /// 连接初始化
        /// </summary>
        private void Init()
        {
            connectionFactory = new ConnectionFactory()
            {
                UserName = config.UserName,
                Password = config.Password,
                HostName = config.HostName,
                Port = int.Parse(config.Port),
                VirtualHost = config.VirtualHost,
                RequestedHeartbeat = TimeSpan.FromSeconds(config.Heartbeat),
                AutomaticRecoveryEnabled = true
            };

            //获取连接
            connection = connectionFactory.CreateConnection();

            //声明通道
            Channel = connection.CreateModel();
        }

        /// <summary>
        /// 处理器释放
        /// </summary>
        public void Dispose()
        {
            EStatus = Enum_Process.Finished;

            Channel.Dispose();
            connection.Dispose();
        }

        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="eMQFunc"></param>
        /// <param name="mqParam"></param>
        public void Run(Enum_MQFunc eMQFunc, TParam mqParam)
        {
            if (!connection.IsOpen)
                Init();

            switch (eMQFunc)
            {
                case Enum_MQFunc.Exec:
                    Exec(mqParam);
                    break;
                case Enum_MQFunc.Quit:
                    Quit(mqParam);
                    break;
            }
        }

        /// <summary>
        /// 执行队列
        /// </summary>
        /// <param name="mqParam"></param>
        protected abstract void Exec(TParam mqParam);

        /// <summary>
        /// 退出队列
        /// </summary>
        /// <param name="mqParam"></param>
        protected abstract void Quit(TParam mqParam);
    }
}
