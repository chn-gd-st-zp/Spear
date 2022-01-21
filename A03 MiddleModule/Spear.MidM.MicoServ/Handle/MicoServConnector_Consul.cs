using System;
using System.Collections.Generic;
using System.Linq;

using Consul;

using Spear.Inf.Core;
using Spear.Inf.Core.Attr;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.Interface;
using Spear.Inf.Core.Tool;

namespace Spear.MidM.MicoServ
{
    [DIModeForService(Enum_DIType.ExclusiveByKeyed, typeof(IMicoServConnector), Enum_RegisCenter.Consul)]
    public class MicoServConnector_Consul : IMicoServConnector
    {
        public readonly MicoServClientSettings micoServClientSettings;

        public MicoServConnector_Consul()
        {
            micoServClientSettings = ServiceContext.Resolve<MicoServClientSettings>();
        }

        public string GenericServAddress(string serverIdentity)
        {
            string servName = serverIdentity;

            ConsulClient consulClient = new ConsulClient(o => o.Address = new Uri(micoServClientSettings.ServAddress));

            CatalogService service = null;
            CatalogService[] services = consulClient.Catalog.Service(servName).Result.Response;
            if (services != null && services.Any())
            {
                //模拟随机一台进行请求，这里只是测试，可以选择合适的负载均衡工具或框架
                service = services.ElementAt(Unique.GetRandom().Next(services.Count()));
            }

            List<DeployMode> hostModes = service.ServiceAddress.ToObject<List<DeployMode>>();
            var hostMode = hostModes.Where(o => o.AccessMode == micoServClientSettings.AccessMode).First();

            return $"http://{ hostMode.Host}:{hostMode.Port}";
        }
    }
}
