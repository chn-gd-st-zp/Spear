﻿using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Grpc.Net.Client;
using MagicOnion.Client;

using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.CusResult;
using Spear.Inf.Core.DTO;
using Spear.Inf.Core.ServGeneric.MicServ;

using Spear.Demo.Inf.DTO;
using Spear.Demo.Support;

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

            result = GetService<IGRPC>()
                .Test1()
                .ResponseAsync.Result;

            result = GetService<IGRPC>()
                .Test2("", "")
                .ResponseAsync.Result;

            result = GetService<IGRPC>(address)
                .Test3(
                    new IDTO_GRPC<IDTO_ListParam>
                    {
                        GRPCContext = new IDTO_GRPCContext() { Token = "" },
                        Param = new IDTO_ListParam()
                    }
                )
                .ResponseAsync.Result;

            result = GetService<IGRPC>(address)
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
                .ResponseAsync.Result;

            return false.ResultWebApi_Fail();
        }
    }
}