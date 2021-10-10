using Microsoft.AspNetCore.Builder;
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
    }

    public class ConfigureCollectionBase : IConfigureCollection
    {
        public IApplicationBuilder App { get; set; }

        public IHostEnvironment Env { get; set; }

        public IHostApplicationLifetime Lifetime { get; set; }

        public ILoggerFactory LoggerFactory { get; set; }
    }
}
