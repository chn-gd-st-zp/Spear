﻿using Autofac;

namespace Spear.MidM.Redis
{
    public static class MidModule_Redis
    {
        public static ContainerBuilder RegisRedis(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<SERedis>().As<ICache4Redis>().SingleInstance();

            return containerBuilder;
        }
    }
}
