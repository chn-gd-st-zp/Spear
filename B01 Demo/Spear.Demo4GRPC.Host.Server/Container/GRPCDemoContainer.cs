using System.Collections.Generic;

using MagicOnion;

using Spear.Inf.Core;
using Spear.Inf.Core.CusResult;
using Spear.Inf.Core.DTO;
using Spear.MidM.MicoServ.MagicOnion;

using Spear.Demo.Support;
using Spear.Demo.Contract;
using Spear.Demo.Inf.DTO;

namespace Spear.Demo4GRPC.Host.Server.MicoServ
{
    public class GRPCDemoContainer : MagicOnion.Server.ServiceBase<IGRPCDemoContainer>, IGRPCDemoContainer
    {
        private readonly IGRPCService _grpcService;

        public GRPCDemoContainer()
        {
            _grpcService = ServiceContext.Resolve<IGRPCService>();
        }

        public UnaryResult<MagicOnionResult<bool>> Test1()
        {
            var result = new ProcessResult<bool>();

            return result.ToUnaryResult();
        }

        public UnaryResult<MagicOnionResult<bool>> Test2(params string[] args)
        {
            var result = new ProcessResult<bool>();

            return result.ToUnaryResult();
        }

        public UnaryResult<MagicOnionResult<bool>> Test3(IDTO_GRPC<IDTO_ListParam> input)
        {
            var result = new ProcessResult<bool>();

            return result.ToUnaryResult();
        }

        public UnaryResult<MagicOnionResult<bool>> Test4(IDTO_GRPC<IDTO_ListParam> input, IDTO_ListParam input2)
        {
            var result = new ProcessResult<bool>();

            return result.ToUnaryResult();
        }

        public UnaryResult<MagicOnionResult<List<ODTOTestDemo>>> List(IDTO_GRPC<IDTO_ListParam> input)
        {
            var result = _grpcService.List(input.Param).Result;

            return result.ToUnaryResult();
        }

        public UnaryResult<MagicOnionResult<ODTO_Page<ODTOTestDemo>>> Page(IDTO_GRPC<IDTO_PageParam> input)
        {
            var result = _grpcService.Page(input.Param).Result;

            return result.ToUnaryResult();
        }

        public UnaryResult<MagicOnionResult<ODTO_Tree<ODTOTestDemo>>> Tree(IDTO_GRPC<IDTO_TreeParam> input)
        {
            var result = _grpcService.Tree(input.Param).Result;

            return result.ToUnaryResult();
        }

        public UnaryResult<MagicOnionResult<List<ODTOTestDemo>>> ImportExcel(IDTO_GRPC<IDTO_Import> input)
        {
            var result = _grpcService.ImportExcel(input.Param).Result;

            return result.ToUnaryResult();
        }

        public UnaryResult<MagicOnionResult<byte[]>> ExportExcel(IDTO_GRPC<IDTO_Export> input)
        {
            var result = _grpcService.ExportExcel(input.Param).Result;

            return result.ToUnaryResult();
        }
    }
}
