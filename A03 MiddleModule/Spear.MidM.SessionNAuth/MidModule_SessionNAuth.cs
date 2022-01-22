using Autofac;

using Spear.Inf.Core.Interface;
using Spear.MidM.Redis;

namespace Spear.MidM.SessionNAuth
{
    public static class MidModule_SessionNAuth
    {
        public static ContainerBuilder RegisSessionNAuth(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisRedis<SessionNAuthSettings>();
            containerBuilder.RegisterGeneric(typeof(SpearSession<>)).As(typeof(ISpearSession<>)).InstancePerDependency();

            return containerBuilder;
        }
    }
}
