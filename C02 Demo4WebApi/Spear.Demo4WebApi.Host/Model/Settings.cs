using Spear.Inf.Core.AppEntrance;
using Spear.MidM.Swagger;

namespace Spear.Demo4WebApi.Host
{
    public class Settings : AppSettingsBase
    {
        public SwaggerSettings SwaggerSettings { get; set; }

        public DBConnectionSettings DBConnectionSettings { get; set; }
    }

    public class DBConnectionSettings
    {
        public string Stainless { get; set; }
    }
}
