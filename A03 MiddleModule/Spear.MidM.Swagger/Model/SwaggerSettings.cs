using Spear.Inf.Core.Attr;
using Spear.Inf.Core.SettingsGeneric;

namespace Spear.MidM.Swagger
{
    [DIModeForSettings("SwaggerSettings", typeof(SwaggerSettings))]
    public class SwaggerSettings : ISettings
    {
        public string RoutePrefix { get; set; } = "swagger";

        public string Title { get; set; }

        public string VersionKeyInQuery { get; set; }

        public string VersionKeyInHeader { get; set; }

        public string AccessTokenKeyInHeader { get; set; }

        public string[] Patterns { get; set; }

        public string[] Xmls { get; set; }
    }
}
