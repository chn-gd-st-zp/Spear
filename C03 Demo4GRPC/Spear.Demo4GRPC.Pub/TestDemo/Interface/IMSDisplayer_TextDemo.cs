using System.Collections.Generic;

using MagicOnion;

using Spear.Inf.Core.CusResult;
using Spear.Inf.Core.DTO;
using Spear.Inf.Core.ServGeneric.MicServ;

namespace Spear.Demo4GRPC.Pub.TestDemo
{
    public interface IMSDisplayer_TextDemo : IMicServ<IMSDisplayer_TextDemo>
    {
        UnaryResult<ResultMicServ<bool>> Test1();

        UnaryResult<ResultMicServ<bool>> Test2(params string[] args);

        UnaryResult<ResultMicServ<bool>> Test3(IDTO_GRPC<ListParam> input);

        UnaryResult<ResultMicServ<bool>> Test4(IDTO_GRPC<ListParam> input, ListParam input2);

        UnaryResult<ResultMicServ<List<ODTOTestDemo>>> List(IDTO_GRPC<ListParam> input);

        UnaryResult<ResultMicServ<ODTO_Page<ODTOTestDemo>>> Page(IDTO_GRPC<PageParam> input);

        UnaryResult<ResultMicServ<ODTO_Tree<ODTOTestDemo>>> Tree(IDTO_GRPC<TreeParam> input);

        UnaryResult<ResultMicServ<List<ODTOTestDemo>>> ImportExcel(IDTO_GRPC<ImportParam> input);

        UnaryResult<ResultMicServ<byte[]>> ExportExcel(IDTO_GRPC<ExportParam> input);
    }
}
