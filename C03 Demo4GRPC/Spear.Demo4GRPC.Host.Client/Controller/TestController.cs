using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Grpc.Net.Client;
using MagicOnion.Client;

using Spear.Inf.Core.Base;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.CusResult;
using Spear.Inf.Core.DTO;
using Spear.Inf.Core.ServGeneric.MicServ;
using Spear.Inf.Core.Tool;
using Spear.GlobalSupport.Base.Interface;
using Spear.Demo4GRPC.Pub.TestDemo;

using ServiceContext = Spear.Inf.Core.ServGeneric.ServiceContext;

namespace Spear.Demo4GRPC.Host.Client.Controller
{
    [Route("test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private T GetService<T>() where T : IMicServ<T>
        {
            return ServiceContext.ResolveMicServ<T>();
        }

        private T GetService<T>(string address) where T : IMicServ<T>
        {
            return MagicOnionClient.Create<T>(GrpcChannel.ForAddress(address));
        }

        [HttpGet, Route("go")]
        public async Task<ResultWebApi<bool>> Go()
        {
            string address = "http://localhost:1001";
            object result = null;

            result = GetService<IMSRunner>()
                .Run()
                .ResponseAsync.Result;

            result = GetService<IMSRunner>(address)
                .Run(new IDTO_GRPC<DateTime> { Param = DateTime.Now }.ToJson())
                .ResponseAsync.Result;

            result = GetService<IMSDisplayer_TextDemo>()
                .Test1()
                .ResponseAsync.Result;

            result = GetService<IMSDisplayer_TextDemo>()
                .Test2("", "")
                .ResponseAsync.Result;

            result = GetService<IMSDisplayer_TextDemo>(address)
                .Test3(
                    new IDTO_GRPC<ListParam>
                    {
                        GRPCContext = new IDTO_GRPCContext() { Token = "" },
                        Param = new ListParam()
                    }
                )
                .ResponseAsync.Result;

            result = GetService<IMSDisplayer_TextDemo>(address)
                .Test4(
                    new IDTO_GRPC<ListParam>
                    {
                        GRPCContext = new IDTO_GRPCContext() { Token = "123" },
                        Param = new ListParam()
                        {
                            EStatus = Enum_Status.Normal,
                        }
                    },
                    new ListParam()
                    {
                        EStatus = Enum_Status.Normal,
                    }
                )
                .ResponseAsync.Result;

            return false.ResultWebApi_Fail();
        }
    }
}
