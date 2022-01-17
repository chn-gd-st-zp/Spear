using System.Collections.Generic;

using MagicOnion;
using MagicOnion.Server;

using Spear.Inf.Core.CusResult;
using Spear.Inf.Core.DTO;

using Spear.Demo.Support;
using Spear.Demo.Contract;
using Spear.Demo.Inf.DTO;

using ServiceContext = Spear.Inf.Core.ServGeneric.ServiceContext;

namespace Spear.Demo4GRPC.Host.Server.MicServ
{
    public class GRPC : ServiceBase<IGRPC>, IGRPC
    {
        private readonly IGRPCService _grpcService;

        public GRPC()
        {
            _grpcService = ServiceContext.Resolve<IGRPCService>();
        }

        public UnaryResult<ResultMicServ<bool>> Test1()
        {
            return true.ResultBase_Success().ToMicServResult();
        }

        public UnaryResult<ResultMicServ<bool>> Test2(params string[] args)
        {
            return true.ResultBase_Success().ToMicServResult();
        }

        public UnaryResult<ResultMicServ<bool>> Test3(IDTO_GRPC<IDTO_ListParam> input)
        {
            return true.ResultBase_Success().ToMicServResult();
        }

        public UnaryResult<ResultMicServ<bool>> Test4(IDTO_GRPC<IDTO_ListParam> input, IDTO_ListParam input2)
        {
            return true.ResultBase_Success().ToMicServResult();
        }

        public UnaryResult<ResultMicServ<List<ODTOTestDemo>>> List(IDTO_GRPC<IDTO_ListParam> input)
        {
            var result = _grpcService.List(input.Param);

            return result.ToMicServResult();
        }

        public UnaryResult<ResultMicServ<ODTO_Page<ODTOTestDemo>>> Page(IDTO_GRPC<IDTO_PageParam> input)
        {
            var result = _grpcService.Page(input.Param);

            return result.ToMicServResult();
        }

        public UnaryResult<ResultMicServ<ODTO_Tree<ODTOTestDemo>>> Tree(IDTO_GRPC<IDTO_TreeParam> input)
        {
            var result = _grpcService.Tree(input.Param);

            return result.ToMicServResult();
        }

        public UnaryResult<ResultMicServ<List<ODTOTestDemo>>> ImportExcel(IDTO_GRPC<IDTO_Import> input)
        {
            var result = _grpcService.ImportExcel(input.Param);

            return result.ToMicServResult();
        }

        public UnaryResult<ResultMicServ<byte[]>> ExportExcel(IDTO_GRPC<IDTO_Export> input)
        {
            var result = _grpcService.ExportExcel(input.Param);

            return result.ToMicServResult();
        }
    }
}
