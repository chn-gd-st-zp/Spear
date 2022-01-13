using Microsoft.Extensions.Hosting;

namespace Spear.MidM.Permission
{
    public static class MidModule_Permission
    {
        public static IHostApplicationLifetime RegisPermission(this IHostApplicationLifetime lifetime)
        {
            var lifeTime = new PermissionLifeTime();

            lifetime.ApplicationStarted.Register(() => lifeTime.Started());
            lifetime.ApplicationStopping.Register(() => lifeTime.Started());
            lifetime.ApplicationStopped.Register(() => lifeTime.Started());

            return lifetime;
        }
    }
}
