using Autofac;

using Spear.Inf.Core.Interface;

namespace Spear.MidM.Redis
{
    public static class MidModule_Redis
    {
        public static ContainerBuilder RegisRedis(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<SERedis>().As<ICache>().SingleInstance();
            containerBuilder.RegisterType<SERedis>().As<ICache4Redis>().SingleInstance();

            return containerBuilder;
        }

        public static ContainerBuilder RegisRedis(this ContainerBuilder containerBuilder, string name) 
        {
            containerBuilder.RegisterType<SERedis>().Named<ICache>(name).SingleInstance();
            containerBuilder.RegisterType<SERedis>().Named<ICache4Redis>(name).SingleInstance();

            return containerBuilder;
        }

        public static ContainerBuilder RegisRedis(this ContainerBuilder containerBuilder, object obj)
        {
            containerBuilder.RegisterType<SERedis>().Keyed<ICache>(obj).SingleInstance();
            containerBuilder.RegisterType<SERedis>().Keyed<ICache4Redis>(obj).SingleInstance();

            return containerBuilder;
        }

        public static ContainerBuilder RegisRedis<T>(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<SERedis>().Keyed<ICache>(typeof(T)).SingleInstance();
            containerBuilder.RegisterType<SERedis>().Keyed<ICache4Redis>(typeof(T)).SingleInstance();

            return containerBuilder;
        }
    }
}
