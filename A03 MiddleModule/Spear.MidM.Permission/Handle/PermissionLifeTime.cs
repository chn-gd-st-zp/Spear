using System.Threading.Tasks;

using Spear.Inf.Core.Interface;
using Spear.Inf.Core.ServGeneric;
using Spear.Inf.Core.ServGeneric.IOC;

namespace Spear.MidM.Permission
{
    public interface IPermissionLifeTime : ISingleton
    {
        void Started(params object[] args);

        void Stopping(params object[] args);

        void Stopped(params object[] args);
    }

    public class PermissionLifeTime : ISpearAppLifeTime
    {
        private IPermissionLifeTime _lifeTime;

        public PermissionLifeTime() { _lifeTime = ServiceContext.Resolve<IPermissionLifeTime>(); }

        public async Task Started(params object[] args) { _lifeTime.Started(args); }

        public async Task Stopping(params object[] args) { _lifeTime.Stopping(args); }

        public async Task Stopped(params object[] args) { _lifeTime.Stopped(args); }
    }
}
