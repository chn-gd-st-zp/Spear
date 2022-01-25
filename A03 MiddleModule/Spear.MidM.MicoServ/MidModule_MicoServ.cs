using System;

using Microsoft.Extensions.DependencyInjection;

using Autofac;

using Spear.Inf.Core.AppEntrance;

namespace Spear.MidM.MicoServ
{
    public static class MidModule_MicoServ
    {
        public static IServiceCollection RegisMicoServHandler(this IServiceCollection services, Action<IServiceCollection> action)
        {
            services.AddGrpc();

            action(services);

            return services;
        }

        public static ContainerBuilder RegisMicoServProvider<TProvider>(this ContainerBuilder containerBuilder)
            where TProvider : IMicoServProvider
        {
            containerBuilder.RegisterType<TProvider>().As<IMicoServProvider>().SingleInstance();

            return containerBuilder;
        }

        public static AppConfiguresBase UseMicoServ(this AppConfiguresBase appConfigures)
        {
            var lifeTime = new MicoServLifeTime();

            appConfigures.Lifetime.ApplicationStarted.Register(() => lifeTime.Started());
            appConfigures.Lifetime.ApplicationStopping.Register(() => lifeTime.Stopping());
            appConfigures.Lifetime.ApplicationStopped.Register(() => lifeTime.Stopped());

            return appConfigures;
        }
    }
}
