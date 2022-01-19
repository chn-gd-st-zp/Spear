using System;
using System.Threading.Tasks;

using Consul;

using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.Interface;
using Spear.Inf.Core.ServGeneric;
using Spear.Inf.Core.Tool;

namespace Spear.MidM.MicoServ
{
    internal class MicoServLifeTime : ISpearAppLifeTime
    {
        private MicoServDeploySettings _micoServDeploySettings;
        private MicoServServerSettings _micoServServerSettings;
        private AgentServiceRegistration _registration;
        private ConsulClient _consulClient;

        internal MicoServLifeTime()
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);

            _micoServDeploySettings = ServiceContext.Resolve<MicoServDeploySettings>();
            _micoServServerSettings = ServiceContext.Resolve<MicoServServerSettings>();

            if (_micoServServerSettings != null && !_micoServServerSettings.ConsulAddress.IsEmptyString())
            {
                var _registration = new AgentServiceRegistration()
                {
                    ID = _micoServServerSettings.NodeName + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                    Name = _micoServServerSettings.NodeName,
                    Address = _micoServServerSettings.GetDeployModes(Enum_Protocol.GRPC, _micoServDeploySettings).ToJson(),

                    //健康监测
                    Check = new AgentServiceCheck()
                    {
                        //服务启动多久后注册
                        DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),

                        //健康检查时间间隔，或者称为心跳间隔
                        Interval = TimeSpan.FromSeconds(10),

                        //超时设置
                        Timeout = TimeSpan.FromSeconds(10),

                        //健康检查地址
                        HTTP = _micoServServerSettings.GetServAddr_WebApi(_micoServDeploySettings) + _micoServServerSettings.HealthCheckRoute,
                    },

                    //添加 urlprefix-/servicename 格式的 tag 标签，以便 Fabio 识别
                    Tags = new[] { $"urlprefix-/{_micoServServerSettings.NodeName} proto=grpc grpcservername={_micoServServerSettings.NodeName}" },
                };

                //创建服务客户端
                _consulClient = new ConsulClient(o => o.Address = new Uri(_micoServServerSettings.ConsulAddress));
            }
        }

        public async Task Started(params object[] args)
        {
            if (_registration != null && _consulClient != null)
                _consulClient.Agent.ServiceRegister(_registration).Wait();
        }

        public async Task Stopping(params object[] args)
        {
            if (_registration != null && _consulClient != null)
                _consulClient.Agent.ServiceDeregister(_registration.ID).Wait();
        }

        public async Task Stopped(params object[] args) { }
    }
}
