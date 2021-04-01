using Autofac;

using Spear.Inf.Core.Interface;

namespace Spear.MidM.RabbitMQ
{
    public static class MidModule_RabbitMQ
    {
        public static ContainerBuilder RegisRabbitMQ(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterGeneric(typeof(RabbitMQService<,>)).As(typeof(IMsgQueueService<,>)).InstancePerDependency();

            return containerBuilder;
        }
    }
}
