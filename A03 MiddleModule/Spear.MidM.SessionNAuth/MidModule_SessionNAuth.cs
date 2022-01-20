using Autofac;

using Spear.Inf.Core.Interface;

namespace Spear.MidM.SessionNAuth
{
    public static class MidModule_SessionNAuth
    {
        public static ContainerBuilder RegisSessionNAuth(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterGeneric(typeof(SpearSession<>)).As(typeof(ISpearSession<>)).InstancePerDependency();

            return containerBuilder;
        }
    }
}
