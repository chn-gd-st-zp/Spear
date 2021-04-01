using System.Collections.Generic;
using System.Linq;
using System.Text;

using RabbitMQ.Client;

using Spear.Inf.Core.Tool;

namespace Spear.MidM.RabbitMQ
{
    public class WorkPublisher : RabbitMQHandle<WorkParamRequest>
    {
        protected override void Exec(WorkParamRequest mqParam)
        {
            if (mqParam.MessageEntities == null || mqParam.MessageEntities.Count() == 0)
                return;

            var arguments = new Dictionary<string, object>();

            if (mqParam.EPriority != Enum_Priority.None)
                arguments["x-max-priority"] = (int)mqParam.EPriority;

            try
            {
                //声明队列
                Channel.QueueDeclareNoWait(
                    queue: mqParam.Queue,
                    durable: mqParam.Durable,
                    exclusive: mqParam.Exclusive,
                    autoDelete: mqParam.AutoDelete,
                    arguments: arguments
                );
            }
            catch (System.Exception ex)
            {
                //
            }

            foreach (var messageEntity in mqParam.MessageEntities)
            {
                var properties = Channel.CreateBasicProperties();
                properties.Persistent = mqParam.Persistent;
                properties.Priority = (byte)(int)mqParam.EMsgPriority;

                //发布消息
                Channel.BasicPublish(
                        exchange: "",
                        routingKey: mqParam.Queue,
                        basicProperties: properties,
                        body: Encoding.UTF8.GetBytes(messageEntity.ToJson())
                    );
            }
        }

        protected override void Quit(WorkParamRequest mqParam)
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
