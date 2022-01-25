using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Spear.Inf.Core.AppEntrance
{
    public interface IConfigureCollection
    {
        IApplicationBuilder App { get; set; }

        IHostEnvironment Env { get; set; }

        IHostApplicationLifetime Lifetime { get; set; }

        ILoggerFactory LoggerFactory { get; set; }

        IApiVersionDescriptionProvider ApiVerDescProvider { get; set; }
    }

    public class AppConfiguresBase : IConfigureCollection
    {
        public IApplicationBuilder App { get; set; }

        public IHostEnvironment Env { get; set; }

        public IHostApplicationLifetime Lifetime { get; set; }

        public ILoggerFactory LoggerFactory { get; set; }

        public IApiVersionDescriptionProvider ApiVerDescProvider { get; set; }
    }
}
