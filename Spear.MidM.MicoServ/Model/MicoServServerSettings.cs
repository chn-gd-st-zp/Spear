using System.Collections.Generic;
using System.Linq;

using Spear.Inf.Core.Attr;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.SettingsGeneric;

namespace Spear.MidM.MicoServ
{
    [DIModeForSettings("MicoServServerSettings", typeof(MicoServServerSettings))]
    public class MicoServServerSettings : ISettings
    {
        public string ConsulAddress { get; set; }

        public string NodeName { get; set; }


        public string WebApiProto { get; set; }

        public Enum_AccessMode WebApiReqMode { get; set; }


        public string GRPCProto { get; set; }

        public Enum_AccessMode GRPCReqMode { get; set; }


        public string HealthCheckRoute { get; set; }


        public string GetServAddr_WebApi(MicoServDeploySettings micServRunSettings)
        {
            var DeployModes = GetDeployModes_WebApi(micServRunSettings);

            string result = $"{WebApiProto}://{DeployModes.First().Host}:{micServRunSettings.WebApiPort}";
            return result;
        }

        public string GetServAddr_MicServ(MicoServDeploySettings micServRunSettings)
        {
            var DeployModes = GetDeployModes_MicServ(micServRunSettings);

            string result = $"{GRPCProto}://{DeployModes.First().Host}:{micServRunSettings.GRPCPort}";
            return result;
        }

        public List<DeployMode> GetDeployModes_WebApi(MicoServDeploySettings micServRunSettings)
        {
            return GetDeployModes(Enum_ServType.WebApi, micServRunSettings);
        }

        public List<DeployMode> GetDeployModes_MicServ(MicoServDeploySettings micServRunSettings)
        {
            return GetDeployModes(Enum_ServType.GRPC, micServRunSettings);
        }

        public List<DeployMode> GetDeployModes(Enum_ServType servType, MicoServDeploySettings micServRunSettings)
        {
            List<DeployMode> result = new List<DeployMode>();

            var reqMode = servType == Enum_ServType.WebApi ? WebApiReqMode : GRPCReqMode;
            var port = servType == Enum_ServType.WebApi ? micServRunSettings.WebApiPort : micServRunSettings.GRPCPort;

            switch (reqMode)
            {
                case Enum_AccessMode.Internal:
                    result.Add(new DeployMode { ReqMode = Enum_AccessMode.Internal, Host = micServRunSettings.HostInternal, Port = port });
                    break;
                case Enum_AccessMode.Public:
                    result.Add(new DeployMode { ReqMode = Enum_AccessMode.Public, Host = micServRunSettings.HostPublic, Port = port });
                    break;
                case Enum_AccessMode.PublicNInternal:
                    result.Add(new DeployMode { ReqMode = Enum_AccessMode.Internal, Host = micServRunSettings.HostInternal, Port = port });
                    result.Add(new DeployMode { ReqMode = Enum_AccessMode.Public, Host = micServRunSettings.HostPublic, Port = port });
                    break;
            }

            return result;
        }
    }
}
