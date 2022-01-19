using Microsoft.Extensions.Hosting;

namespace Spear.MidM.MicoServ
{
    public static class MidModule_MicoServ
    {
        public static IHostApplicationLifetime RegisMicoServ(this IHostApplicationLifetime lifetime)
        {
            var lifeTime = new MicoServLifeTime();

            lifetime.ApplicationStarted.Register(() => lifeTime.Started());
            lifetime.ApplicationStopping.Register(() => lifeTime.Stopping());
            lifetime.ApplicationStopped.Register(() => lifeTime.Stopped());

            return lifetime;
        }
    }
}
