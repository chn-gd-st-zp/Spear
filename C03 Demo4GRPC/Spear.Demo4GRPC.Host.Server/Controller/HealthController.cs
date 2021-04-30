using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Spear.Inf.Core.Attr;
using Spear.Inf.Core.Basic;
using Spear.Inf.Core.ServGeneric;
using Spear.Inf.Core.ServGeneric.MicServ;
using Spear.Inf.Core.Tool;

namespace Spear.Demo4GRPC.Host.Server.Controller
{
    [Route("health")]
    [ApiController]
    public class HealthController : ControllerBasic
    {
        private readonly MicServRunSettings _micServRunSettings;
        private readonly MicServServerSettings _micServServerSettings;

        public HealthController()
        {
            _micServRunSettings = ServiceContext.Resolve<MicServRunSettings>();
            _micServServerSettings = ServiceContext.Resolve<MicServServerSettings>();
        }

        /// <summary>
        /// 心跳检测
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("check")]
        [LogIgnore]
        public async Task<string> Check()
        {
            string result = $"{_micServServerSettings.NodeName}-{_micServServerSettings.GetHostModes_WebApi(_micServRunSettings).ToJson()} say : im health";
            return result;
        }
    }
}
