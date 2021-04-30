using Spear.Inf.Core.AppEntrance;
using Spear.Inf.Core.ServGeneric.MicServ;

namespace Spear.Demo4GRPC.Host.Client
{
    public class Settings : AppSettingsBasic
    {
        public MicServClientSettings MicServClientSettings { get; set; }
    }
}
