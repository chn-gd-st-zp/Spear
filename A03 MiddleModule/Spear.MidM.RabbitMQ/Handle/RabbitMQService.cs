using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.Interface;

namespace Spear.MidM.RabbitMQ
{
    /// <summary>
    /// MQ接收消息时，所要执行的业务逻辑
    /// </summary>
    /// <param name="channel"></param>
    /// <param name="sender"></param>
    /// <param name="deliverEvent"></param>
    public delegate void MQReceive(IModel channel, object sender, BasicDeliverEventArgs deliverEvent);

    public class RabbitMQService<THandle, TParam> : IMsgQueueService<THandle, TParam>
        where THandle : RabbitMQHandle<TParam>, new()
        where TParam : class, IMsgQueueParam
    {
        private readonly THandle _service;
        private TParam _mqParam;

        public RabbitMQService()
        {
            _service = new THandle();
        }

        /// <summary>
        /// 释放服务
        /// </summary>
        public void Dispose()
        {
            if (_service == null || _service.EStatus == Enum_Process.Finished)
                return;

            _service.Dispose();
        }

        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="eMQFunc"></param>
        /// <param name="mqParam"></param>
        public void Run(Enum_MQFunc eMQFunc, TParam mqParam = null)
        {
            if (_service == null || _service.EStatus == Enum_Process.Finished)
                return;

            _mqParam = mqParam == null ? _mqParam : mqParam;

            _service.Run(eMQFunc, _mqParam);
        }
    }
}
