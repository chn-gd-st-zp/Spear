using Spear.Inf.Core.AppEntrance;
using Spear.MidM.MicoServ;
using Spear.MidM.Swagger;

namespace Spear.Demo4GRPC.Host.Server
{
    public class Settings : AppSettingsBase
    {
        public SwaggerSettings SwaggerSettings { get; set; }

        public MicoServServerSettings MicoServServerSettings { get; set; }

        public MicoServDeploySettings MicoServDeploySettings { get; set; }
    }
}
