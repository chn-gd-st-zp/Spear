using Autofac;

namespace Spear.MidM.SessionNAuth
{
    public static class MidModule_SessionNAuth
    {
        public static ContainerBuilder RegisSessionNAuth(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<HTTPTokenProvider>().AsSelf().InstancePerLifetimeScope();
            containerBuilder.RegisterType<GRPCTokenProvider>().AsSelf().InstancePerLifetimeScope();

            containerBuilder.RegisterGeneric(typeof(SessionNAuth<>)).As(typeof(ISessionNAuth<>)).InstancePerLifetimeScope();

            return containerBuilder;
        }
    }
}
