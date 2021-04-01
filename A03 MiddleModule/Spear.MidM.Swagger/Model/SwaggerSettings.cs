using System.Collections.Generic;

namespace Spear.MidM.Swagger
{
    public class SwaggerSettings
    {
        public string Code { get; set; }

        public string Title { get; set; }

        public string Version { get; set; }

        public string DefaultPattern { get; set; }

        public List<string> Xmls { get; set; }
    }
}
