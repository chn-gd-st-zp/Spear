using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Spear.Inf.Core;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.CusResult;
using Spear.Inf.Core.DTO;
using Spear.Inf.Core.Interface;
using Spear.MidM.MicoServ;
using Spear.MidM.MicoServ.MagicOnion;

using Spear.Demo.Inf.DTO;
using Spear.Demo.Support;

namespace Spear.Demo4GRPC.Host.Client.Controller
{
    [ApiVersion("1.0", Deprecated = true)]
    [ApiController, Route("api/[controller]"), Route("api/v{version:apiVersion}/[controller]")]
    public class GRPCDemoController : ControllerBase
    {
        private TContainer GetService<TContainer>() where TContainer : IMagicOnionContainer<TContainer>
        {
            var result = default(TContainer);

            var regisCenter = ServiceContext.Resolve<MicoServClientSettings>().RegisCenter;
            var serverIdentity = string.Empty;

            regisCenter = Enum_RegisCenter.Normal;
            serverIdentity = "http://localhost:1001";
            result = MicoServContext.Resolve<TContainer>(regisCenter, serverIdentity);

            //regisCenter = Enum_RegisCenter.Consul;
            //serverIdentity = "Demo4GRPC";
            //result = MicoServContext.Resolve<TContainer>(regisCenter, serverIdentity);

            return result;
        }

        [HttpGet, Route("go")]
        public async Task<WebApiResult<List<object>>> Go()
        {
            var result = new WebApiResult<List<object>>();
            result.Code = ISpearEnum.Restore<IStateCode>().Success;
            result.Data = new List<object>();

            result.Data.Add(
                GetService<IGRPCDemoContainer>()
                .Test3(
                    new IDTO_GRPC<IDTO_ListParam>
                    {
                        GRPCContext = new IDTO_GRPCContext() { Token = string.Empty },
                        Param = new IDTO_ListParam()
                    }
                )
                .ResponseAsync.Result
            );

            result.Data.Add(
                GetService<IGRPCDemoContainer>()
                .Test4(
                    new IDTO_GRPC<IDTO_ListParam>
                    {
                        GRPCContext = new IDTO_GRPCContext() { Token = "123" },
                        Param = new IDTO_ListParam()
                        {
                            EStatus = Enum_Status.Normal,
                        }
                    },
                    new IDTO_ListParam()
                    {
                        EStatus = Enum_Status.Normal,
                    }
                )
                .ResponseAsync.Result
            );

            return result;
        }
    }
}
