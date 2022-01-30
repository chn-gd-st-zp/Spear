using Spear.Inf.Core.SettingsGeneric;
using Spear.MidM.MicoServ;
using Spear.MidM.Swagger;

namespace Spear.Demo4GRPC.Host.Client
{
    public class Settings : AppSettingsBase
    {
        public SwaggerSettings SwaggerSettings { get; set; }

        public MicoServClientSettings MicoServClientSettings { get; set; }
    }
}
