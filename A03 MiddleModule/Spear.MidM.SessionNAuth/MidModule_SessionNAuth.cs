using Autofac;

namespace Spear.MidM.SessionNAuth
{
    public static class MidModule_SessionNAuth
    {
        public static ContainerBuilder RegisSessionNAuth(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<HTTPTokenProvider>().AsSelf().InstancePerDependency();
            containerBuilder.RegisterType<GRPCTokenProvider>().AsSelf().InstancePerDependency();

            containerBuilder.RegisterGeneric(typeof(SessionNAuth<>)).As(typeof(ISessionNAuth<>)).InstancePerDependency();

            return containerBuilder;
        }
    }
}
