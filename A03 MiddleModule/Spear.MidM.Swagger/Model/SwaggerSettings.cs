using System.Collections.Generic;

using Spear.Inf.Core.Attr;
using Spear.Inf.Core.SettingsGeneric;

namespace Spear.MidM.Swagger
{
    [DIModeForSettings("SwaggerSettings", typeof(SwaggerSettings))]
    public class SwaggerSettings : ISettings
    {
        public string Code { get; set; }

        public string Title { get; set; }

        public string Version { get; set; }

        public string AccessTokenKeyInHeader { get; set; }

        public string DefaultPattern { get; set; }

        public List<string> Xmls { get; set; }
    }
}
