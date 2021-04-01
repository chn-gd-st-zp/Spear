using System.Collections.Generic;
using System.Linq;
using System.Text;

using RabbitMQ.Client;

using Spear.Inf.Core.Tool;

namespace Spear.MidM.RabbitMQ
{
    public class ObserverPublisher : RabbitMQHandle<ObserverParamRequest>
    {
        protected override void Exec(ObserverParamRequest mqParam)
        {
            if (mqParam.MessageEntities == null || mqParam.MessageEntities.Count() == 0)
                return;

            var arguments = new Dictionary<string, object>();

            if (mqParam.EPriority != Enum_Priority.None)
                arguments["x-max-priority"] = (int)mqParam.EPriority;

            //声明交换器
            Channel.ExchangeDeclareNoWait(
                exchange: mqParam.Exchange,
                type: mqParam.EExchangeType.ToString().ToLower(),
                durable: mqParam.Durable,
                autoDelete: mqParam.AutoDelete,
                arguments: arguments
            );

            foreach (var messageEntity in mqParam.MessageEntities)
            {
                var properties = Channel.CreateBasicProperties();
                properties.Persistent = mqParam.Persistent;
                properties.Priority = (byte)(int)mqParam.EMsgPriority;

                //发布消息
                Channel.BasicPublish(
                        exchange: mqParam.Exchange,
                        routingKey: "",
                        basicProperties: properties,
                        body: Encoding.UTF8.GetBytes(messageEntity.ToJson())
                    );
            }
        }

        protected override void Quit(ObserverParamRequest mqParam)
        {
            var arguments = new Dictionary<string, object>();

            if (mqParam.EPriority != Enum_Priority.None)
                arguments["x-max-priority"] = (int)mqParam.EPriority;

            Channel.ExchangeDelete(
                exchange: mqParam.Exchange
            //,ifUnused: true
            );
        }
    }
}
