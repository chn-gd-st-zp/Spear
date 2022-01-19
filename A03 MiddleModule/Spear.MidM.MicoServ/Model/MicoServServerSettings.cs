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

        public Enum_AccessMode WebApiAccessMode { get; set; }


        public string GRPCProto { get; set; }

        public Enum_AccessMode GRPCAccessMode { get; set; }


        public string HealthCheckRoute { get; set; }


        public string GetServAddr_WebApi(MicoServDeploySettings micoServDeploySettings)
        {
            var DeployModes = GetDeployModes_WebApi(micoServDeploySettings);

            string result = $"{WebApiProto}://{DeployModes.First().Host}:{micoServDeploySettings.WebApiPort}";
            return result;
        }

        public string GetServAddr_MicoServ(MicoServDeploySettings micoServDeploySettings)
        {
            var DeployModes = GetDeployModes_MicoServ(micoServDeploySettings);

            string result = $"{GRPCProto}://{DeployModes.First().Host}:{micoServDeploySettings.GRPCPort}";
            return result;
        }

        public List<DeployMode> GetDeployModes_WebApi(MicoServDeploySettings micoServDeploySettings)
        {
            return GetDeployModes(Enum_Protocol.HTTP, micoServDeploySettings);
        }

        public List<DeployMode> GetDeployModes_MicoServ(MicoServDeploySettings micoServDeploySettings)
        {
            return GetDeployModes(Enum_Protocol.GRPC, micoServDeploySettings);
        }

        public List<DeployMode> GetDeployModes(Enum_Protocol servType, MicoServDeploySettings micoServDeploySettings)
        {
            List<DeployMode> result = new List<DeployMode>();

            var accessMode = servType == Enum_Protocol.HTTP ? WebApiAccessMode : GRPCAccessMode;
            var port = servType == Enum_Protocol.HTTP ? micoServDeploySettings.WebApiPort : micoServDeploySettings.GRPCPort;

            switch (accessMode)
            {
                case Enum_AccessMode.Internal:
                    result.Add(new DeployMode { AccessMode = Enum_AccessMode.Internal, Host = micoServDeploySettings.HostInternal, Port = port });
                    break;
                case Enum_AccessMode.Public:
                    result.Add(new DeployMode { AccessMode = Enum_AccessMode.Public, Host = micoServDeploySettings.HostPublic, Port = port });
                    break;
                case Enum_AccessMode.PublicNInternal:
                    result.Add(new DeployMode { AccessMode = Enum_AccessMode.Internal, Host = micoServDeploySettings.HostInternal, Port = port });
                    result.Add(new DeployMode { AccessMode = Enum_AccessMode.Public, Host = micoServDeploySettings.HostPublic, Port = port });
                    break;
            }

            return result;
        }
    }
}
