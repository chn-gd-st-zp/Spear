using System;
using System.Collections.Generic;
using System.Linq;

using Consul;

using Spear.Inf.Core.Tool;

namespace Spear.Inf.Core.ServGeneric.MicServ
{
    public class MicServGener_Consul : IMicServGener
    {
        public readonly MicServClientSettings micServClientSettings;

        public MicServGener_Consul()
        {
            micServClientSettings = ServiceContext.Resolve<MicServClientSettings>();
        }

        public T GetServ<T>(params object[] paramArray) where T : IMicServ<T>
        {
            string name = paramArray[0].ToString();

            ConsulClient consulClient = new ConsulClient(o => o.Address = new Uri(micServClientSettings.Address));

            CatalogService service = null;
            CatalogService[] services = consulClient.Catalog.Service(name).Result.Response;
            if (services != null && services.Any())
            {
                //模拟随机一台进行请求，这里只是测试，可以选择合适的负载均衡工具或框架
                service = services.ElementAt(Unique.GetRandom().Next(services.Count()));
            }

            List<HostMode> hostModes = service.ServiceAddress.ToObject<List<HostMode>>();
            var hostMode = hostModes.Where(o => o.ReqMode == micServClientSettings.ReqMode).First();

            string address = "http://" + hostMode.Host + ":" + hostMode.Port;
            var target = new MicServGener_Normal().GetServ<T>(address);
            return target;
        }
    }
}
