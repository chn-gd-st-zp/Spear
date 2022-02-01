using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.Configuration;

using Autofac;

using Spear.Inf.Core.Tool;

namespace Spear.MidM.MicoServ
{
    public static class MicoServExtend
    {
        public static void LoadMicoServDeploySettings(IConfigurationBuilder configBuilder, string[] args)
        {
            if (args.Length > 0)
            {
                string hostPublic = args.Where(o => o.StartsWith("hostpublic=", StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                string hostInternal = args.Where(o => o.StartsWith("hostinternal=", StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                string webApiPort = args.Where(o => o.StartsWith("webapiport=", StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                string micoServPort = args.Where(o => o.StartsWith("micoservport=", StringComparison.OrdinalIgnoreCase)).SingleOrDefault();

                var settings = new MicoServDeploySettings()
                {
                    HostPublic = hostPublic.IsEmptyString() ? string.Empty : hostPublic.Replace("hostpublic=", string.Empty),
                    HostInternal = hostInternal.IsEmptyString() ? string.Empty : hostInternal.Replace("hostinternal=", string.Empty),
                    WebApiPort = webApiPort.IsEmptyString() ? 0 : int.Parse(webApiPort.Replace("webapiport=", string.Empty, StringComparison.OrdinalIgnoreCase)),
                    GRPCPort = micoServPort.IsEmptyString() ? 0 : int.Parse(micoServPort.Replace("micoservport=", string.Empty, StringComparison.OrdinalIgnoreCase)),
                };

                var source = new Dictionary<string, string>
                {
                    ["MicoServDeploySettings:HostPublic"] = settings.HostPublic,
                    ["MicoServDeploySettings:HostInternal"] = settings.HostInternal,
                    ["MicoServDeploySettings:WebApiPort"] = settings.WebApiPort.ToString(),
                    ["MicoServDeploySettings:GRPCPort"] = settings.GRPCPort.ToString(),
                };

                configBuilder.AddInMemoryCollection(source);
            }
        }
    }
}
