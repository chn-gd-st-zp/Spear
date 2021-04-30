using Spear.Inf.Core.AppEntrance;
using Spear.Inf.Core.ServGeneric.MicServ;
using Spear.MidM.Swagger;

namespace Spear.Demo4GRPC.Host.Server
{
    public class Settings : AppSettingsBasic
    {
        public SwaggerSettings SwaggerSettings { get; set; }

        public MicServServerSettings MicServServerSettings { get; set; }

        public MicServRunSettings MicServRunSettings { get; set; }
    }
}
