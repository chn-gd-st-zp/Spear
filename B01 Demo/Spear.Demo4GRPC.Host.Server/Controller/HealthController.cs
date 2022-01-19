using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Spear.Inf.Core.ServGeneric;
using Spear.Inf.Core.Tool;
using Spear.MidM.MicoServ;

namespace Spear.Demo4GRPC.Host.Server.Controller
{
    [ApiController, Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly MicoServDeploySettings _micoServDeploySettings;
        private readonly MicoServServerSettings _micoServServerSettings;

        public HealthController()
        {
            _micoServDeploySettings = ServiceContext.Resolve<MicoServDeploySettings>();
            _micoServServerSettings = ServiceContext.Resolve<MicoServServerSettings>();
        }

        /// <summary>
        /// 心跳检测
        /// </summary>
        /// <returns></returns>
        //[LogIgnore]
        [HttpGet, Route("Check")]
        public async Task<string> Check()
        {
            string result = $"{_micoServServerSettings.NodeName}-{_micoServServerSettings.GetDeployModes_WebApi(_micoServDeploySettings).ToJson()} say : im health";
            return result;
        }
    }
}
