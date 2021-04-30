using System.Collections.Generic;

using MagicOnion;
using MagicOnion.Server;

using Spear.Inf.Core.CusResult;
using Spear.Inf.Core.DTO;
using Spear.Demo4GRPC.Pub.Basic;
using Spear.Demo4GRPC.Pub.TestDemo;

using ServiceContext = Spear.Inf.Core.ServGeneric.ServiceContext;

namespace Spear.Demo4GRPC.Host.Server.MicServ
{
    public class DisplayerMicServ : ServiceBase<IMSDisplayer_TextDemo>, IMSDisplayer_TextDemo
    {
        private readonly IDisplayer_TextDemo _displayer;

        public DisplayerMicServ()
        {
            _displayer = ServiceContext.Resolve<IDataProvider>().Displayer as IDisplayer_TextDemo;
        }

        public UnaryResult<ResultMicServ<bool>> Test1()
        {
            return true.ResultBasic_Success().ToMicServResult();
        }

        public UnaryResult<ResultMicServ<bool>> Test2(params string[] args)
        {
            return true.ResultBasic_Success().ToMicServResult();
        }

        public UnaryResult<ResultMicServ<bool>> Test3(IDTO_GRPC<ListParam> input)
        {
            return true.ResultBasic_Success().ToMicServResult();
        }

        public UnaryResult<ResultMicServ<bool>> Test4(IDTO_GRPC<ListParam> input, ListParam input2)
        {
            return true.ResultBasic_Success().ToMicServResult();
        }

        public UnaryResult<ResultMicServ<List<ODTOTestDemo>>> List(IDTO_GRPC<ListParam> input)
        {
            var result = _displayer.List(input.Param);

            return result.ToMicServResult();
        }

        public UnaryResult<ResultMicServ<ODTO_Page<ODTOTestDemo>>> Page(IDTO_GRPC<PageParam> input)
        {
            var result = _displayer.Page(input.Param);

            return result.ToMicServResult();
        }

        public UnaryResult<ResultMicServ<ODTO_Tree<ODTOTestDemo>>> Tree(IDTO_GRPC<TreeParam> input)
        {
            var result = _displayer.Tree(input.Param);

            return result.ToMicServResult();
        }

        public UnaryResult<ResultMicServ<List<ODTOTestDemo>>> ImportExcel(IDTO_GRPC<ImportParam> input)
        {
            var result = _displayer.ImportExcel(input.Param);

            return result.ToMicServResult();
        }

        public UnaryResult<ResultMicServ<byte[]>> ExportExcel(IDTO_GRPC<ExportParam> input)
        {
            var result = _displayer.ExportExcel(input.Param);

            return result.ToMicServResult();
        }
    }
}
