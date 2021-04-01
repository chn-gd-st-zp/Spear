using System.Collections.Generic;
using System.Linq;
using System.Text;

using RabbitMQ.Client;

using Spear.Inf.Core.Tool;

namespace Spear.MidM.RabbitMQ
{
    public class RoutePublisher : RabbitMQHandle<RouteParamRequest>
    {
        protected override void Exec(RouteParamRequest mqParam)
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
                    routingKey: mqParam.RoutingKey,
                    basicProperties: properties,
                    body: Encoding.UTF8.GetBytes(messageEntity.ToJson())
                );
            }
        }

        protected override void Quit(RouteParamRequest mqParam)
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
