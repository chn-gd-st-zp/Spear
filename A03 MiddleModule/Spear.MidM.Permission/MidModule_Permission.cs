using Microsoft.Extensions.Hosting;

using Autofac;

namespace Spear.MidM.Permission
{
    public static class MidModule_Permission
    {
        public static ContainerBuilder RegisPermission<TPermissionEnum>(this ContainerBuilder containerBuilder)
        {
            containerBuilder.Register(o => new PermissionEnum(typeof(TPermissionEnum))).As<IPermissionEnum>().SingleInstance();

            return containerBuilder;
        }

        public static IHostApplicationLifetime RegisPermission<TPermissionEnum>(this IHostApplicationLifetime lifetime)
        {


            var lifeTime = new PermissionLifeTime();

            lifetime.ApplicationStarted.Register(() => lifeTime.Started());
            lifetime.ApplicationStopping.Register(() => lifeTime.Stopping());
            lifetime.ApplicationStopped.Register(() => lifeTime.Stopped());

            return lifetime;
        }
    }
}
