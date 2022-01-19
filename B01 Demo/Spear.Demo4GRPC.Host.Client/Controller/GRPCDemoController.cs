using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.CusResult;
using Spear.Inf.Core.DTO;
using Spear.MidM.MicoServ;
using Spear.MidM.MicoServ.MagicOnion;

using Spear.Demo.Inf.DTO;
using Spear.Demo.Support;

namespace Spear.Demo4GRPC.Host.Client.Controller
{
    [ApiController, Route("api/[controller]")]
    public class GRPCDemoController : ControllerBase
    {
        private TContainer GetService<TContainer>(string serverIdentity) where TContainer : IMagicOnionContainer<TContainer>
        {
            var service = MicoServContext.Resolve<TContainer>(Enum_RegisCenter.Normal, serverIdentity);
            //var service = return MicoServContext.Resolve<TContainer>(Enum_RegisCenter.Consul, "TestDemo");

            return service;
        }

        [HttpGet, Route("go")]
        public async Task<ResultWebApi<bool>> Go()
        {
            string address = "http://localhost:1001";
            object result = null;

            var aaa = GetService<IGRPCDemoContainer>(address);
            result = aaa.Test1().ResponseAsync.Result;

            result = GetService<IGRPCDemoContainer>(address)
                .Test3(
                    new IDTO_GRPC<IDTO_ListParam>
                    {
                        GRPCContext = new IDTO_GRPCContext() { Token = "" },
                        Param = new IDTO_ListParam()
                    }
                )
                .ResponseAsync.Result;

            //result = GetService<IGRPCDemoContainer>(address)
            //    .Test4(
            //        new IDTO_GRPC<IDTO_ListParam>
            //        {
            //            GRPCContext = new IDTO_GRPCContext() { Token = "123" },
            //            Param = new IDTO_ListParam()
            //            {
            //                EStatus = Enum_Status.Normal,
            //            }
            //        },
            //        new IDTO_ListParam()
            //        {
            //            EStatus = Enum_Status.Normal,
            //        }
            //    )
            //    .ResponseAsync.Result;

            return false.ResultWebApi_Fail();
        }
    }
}
