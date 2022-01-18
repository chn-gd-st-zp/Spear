using System;
using System.Collections.Generic;
using System.Linq;

using Consul;

using Spear.Inf.Core.Interface;
using Spear.Inf.Core.ServGeneric;
using Spear.Inf.Core.Tool;

namespace Spear.MidM.MicoServ
{
    public class MicServProvider_Consul : IMicoServProvider
    {
        public readonly MicoServClientSettings micoServClientSettings;

        public MicServProvider_Consul()
        {
            micoServClientSettings = ServiceContext.Resolve<MicoServClientSettings>();
        }

        public TContainer Resolve<TContainer>(params object[] paramArray) where TContainer : IMicoServContainer
        {
            string name = paramArray[0].ToString();

            ConsulClient consulClient = new ConsulClient(o => o.Address = new Uri(micoServClientSettings.Address));

            CatalogService service = null;
            CatalogService[] services = consulClient.Catalog.Service(name).Result.Response;
            if (services != null && services.Any())
            {
                //模拟随机一台进行请求，这里只是测试，可以选择合适的负载均衡工具或框架
                service = services.ElementAt(Unique.GetRandom().Next(services.Count()));
            }

            List<DeployMode> hostModes = service.ServiceAddress.ToObject<List<DeployMode>>();
            var hostMode = hostModes.Where(o => o.ReqMode == micoServClientSettings.ReqMode).First();

            string address = "http://" + hostMode.Host + ":" + hostMode.Port;
            var target = new MicServProvider_Normal().Resolve<TContainer>(address);
            return target;
        }
    }
}
