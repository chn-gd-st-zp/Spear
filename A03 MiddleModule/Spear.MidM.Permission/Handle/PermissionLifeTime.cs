using System.Threading.Tasks;

using Spear.Inf.Core.Interface;

namespace Spear.MidM.Permission
{
    public class PermissionLifeTime : ISpearAppLifeTime
    {
        public Task Started(params object[] args)
        {
            return Task.CompletedTask;
        }

        public Task Stopping(params object[] args)
        {
            return Task.CompletedTask;
        }

        public Task Stopped(params object[] args)
        {
            return Task.CompletedTask;
        }
    }
}
