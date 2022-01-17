using System.Collections.Generic;

using MagicOnion;

using Spear.Inf.Core.CusResult;
using Spear.Inf.Core.DTO;
using Spear.Inf.Core.ServGeneric.MicServ;

using Spear.Demo.Inf.DTO;

namespace Spear.Demo.Support
{
    public interface IGRPC : IMicServ<IGRPC>
    {
        UnaryResult<ResultMicServ<bool>> Test1();

        UnaryResult<ResultMicServ<bool>> Test2(params string[] args);

        UnaryResult<ResultMicServ<bool>> Test3(IDTO_GRPC<IDTO_ListParam> input);

        UnaryResult<ResultMicServ<bool>> Test4(IDTO_GRPC<IDTO_ListParam> input, IDTO_ListParam input2);

        UnaryResult<ResultMicServ<List<ODTOTestDemo>>> List(IDTO_GRPC<IDTO_ListParam> input);

        UnaryResult<ResultMicServ<ODTO_Page<ODTOTestDemo>>> Page(IDTO_GRPC<IDTO_PageParam> input);

        UnaryResult<ResultMicServ<ODTO_Tree<ODTOTestDemo>>> Tree(IDTO_GRPC<IDTO_TreeParam> input);

        UnaryResult<ResultMicServ<List<ODTOTestDemo>>> ImportExcel(IDTO_GRPC<IDTO_Import> input);

        UnaryResult<ResultMicServ<byte[]>> ExportExcel(IDTO_GRPC<IDTO_Export> input);
    }
}
