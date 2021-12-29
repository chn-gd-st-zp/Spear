using System.Collections.Generic;
using System.Linq;

using Spear.Inf.Core.Attr;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.SettingsGeneric;

namespace Spear.Inf.Core.ServGeneric.MicServ
{
    public enum Enum_ReqMode
    {
        None,
        Public,
        Internal,
        PublicNInternal,
    }

    public enum Enum_MSType
    {
        None,
        Normal,
        Consul,
    }

    public class HostMode
    {
        public Enum_ReqMode ReqMode { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }
    }

    [DIModeForSettings("MicServRunSettings", typeof(MicServRunSettings))]
    public class MicServRunSettings : ISettings
    {
        public string HostPublic { get; set; }

        public string HostInternal { get; set; }

        public int WebApiPort { get; set; }

        public int GRPCPort { get; set; }
    }

    [DIModeForSettings("MicServClientSettings", typeof(MicServClientSettings))]
    public class MicServClientSettings : ISettings
    {
        public Enum_MSType MSType { get; set; }

        public Enum_ReqMode ReqMode { get; set; }

        public string Address { get; set; }
    }

    [DIModeForSettings("MicServServerSettings", typeof(MicServServerSettings))]
    public class MicServServerSettings : ISettings
    {
        public string ConsulAddress { get; set; }

        public string NodeName { get; set; }


        public string WebApiProto { get; set; }

        public Enum_ReqMode WebApiReqMode { get; set; }


        public string GRPCProto { get; set; }

        public Enum_ReqMode GRPCReqMode { get; set; }


        public string HealthCheckRoute { get; set; }


        public string GetServAddr_WebApi(MicServRunSettings micServRunSettings)
        {
            var hostModes = GetHostModes_WebApi(micServRunSettings);

            string result = $"{WebApiProto}://{hostModes.First().Host}:{micServRunSettings.WebApiPort}";
            return result;
        }

        public string GetServAddr_MicServ(MicServRunSettings micServRunSettings)
        {
            var hostModes = GetHostModes_MicServ(micServRunSettings);

            string result = $"{GRPCProto}://{hostModes.First().Host}:{micServRunSettings.GRPCPort}";
            return result;
        }

        public List<HostMode> GetHostModes_WebApi(MicServRunSettings micServRunSettings)
        {
            return GetHostModes(Enum_ServType.WebApi, micServRunSettings);
        }

        public List<HostMode> GetHostModes_MicServ(MicServRunSettings micServRunSettings)
        {
            return GetHostModes(Enum_ServType.GRPC, micServRunSettings);
        }

        public List<HostMode> GetHostModes(Enum_ServType servType, MicServRunSettings micServRunSettings)
        {
            List<HostMode> result = new List<HostMode>();

            var reqMode = servType == Enum_ServType.WebApi ? WebApiReqMode : GRPCReqMode;
            var port = servType == Enum_ServType.WebApi ? micServRunSettings.WebApiPort : micServRunSettings.GRPCPort;

            switch (reqMode)
            {
                case Enum_ReqMode.Internal:
                    result.Add(new HostMode { ReqMode = Enum_ReqMode.Internal, Host = micServRunSettings.HostInternal, Port = port });
                    break;
                case Enum_ReqMode.Public:
                    result.Add(new HostMode { ReqMode = Enum_ReqMode.Public, Host = micServRunSettings.HostPublic, Port = port });
                    break;
                case Enum_ReqMode.PublicNInternal:
                    result.Add(new HostMode { ReqMode = Enum_ReqMode.Internal, Host = micServRunSettings.HostInternal, Port = port });
                    result.Add(new HostMode { ReqMode = Enum_ReqMode.Public, Host = micServRunSettings.HostPublic, Port = port });
                    break;
            }

            return result;
        }
    }
}
