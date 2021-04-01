using System;
using System.Collections.Generic;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Spear.MidM.RabbitMQ
{
    public class WorkConsumer : RabbitMQHandle<WorkParamResponse>
    {
        protected override void Exec(WorkParamResponse mqParam)
        {
            if (mqParam.BusinessFunc == null)
                return;

            var arguments = new Dictionary<string, object>();

            if (mqParam.EPriority != Enum_Priority.None)
                arguments["x-max-priority"] = (int)mqParam.EPriority;

            //声明队列
            Channel.QueueDeclareNoWait(
                queue: mqParam.Queue,
                durable: mqParam.Durable,
                exclusive: mqParam.Exclusive,
                autoDelete: mqParam.AutoDelete,
                arguments: arguments
            );

            //客服端 向 服务器 索取的消息条目数
            Channel.BasicQos(0, Convert.ToUInt16(mqParam.PrefetchCount), false);

            //定义队列的消费者
            var consumer = new EventingBasicConsumer(Channel);
            consumer.Received += (sender, e) =>
            {
                mqParam.BusinessFunc(Channel, sender, e);
            };

            //监听队列
            Channel.BasicConsume(
                    queue: mqParam.Queue,
                    autoAck: mqParam.AutoAck,
                    consumer: consumer
                );
        }

        protected override void Quit(WorkParamResponse mqParam)
        {
            var arguments = new Dictionary<string, object>();

            if (mqParam.EPriority != Enum_Priority.None)
                arguments["x-max-priority"] = (int)mqParam.EPriority;

            Channel.QueueDelete(
                queue: mqParam.Queue
            //,ifUnused: true,
            //,ifEmpty: true
            );
        }
    }
}
