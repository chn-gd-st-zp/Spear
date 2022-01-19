using System.Collections.Generic;

using MagicOnion;

using Spear.Inf.Core.DTO;
using Spear.MidM.MicoServ.MagicOnion;

using Spear.Demo.Inf.DTO;

namespace Spear.Demo.Support
{
    public interface IGRPCDemoContainer : IMagicOnionContainer<IGRPCDemoContainer>
    {
        UnaryResult<MagicOnionResult<bool>> Test1();

        UnaryResult<MagicOnionResult<bool>> Test2(params string[] args);

        UnaryResult<MagicOnionResult<bool>> Test3(IDTO_GRPC<IDTO_ListParam> input);

        UnaryResult<MagicOnionResult<bool>> Test4(IDTO_GRPC<IDTO_ListParam> input, IDTO_ListParam input2);

        UnaryResult<MagicOnionResult<List<ODTOTestDemo>>> List(IDTO_GRPC<IDTO_ListParam> input);

        UnaryResult<MagicOnionResult<ODTO_Page<ODTOTestDemo>>> Page(IDTO_GRPC<IDTO_PageParam> input);

        UnaryResult<MagicOnionResult<ODTO_Tree<ODTOTestDemo>>> Tree(IDTO_GRPC<IDTO_TreeParam> input);

        UnaryResult<MagicOnionResult<List<ODTOTestDemo>>> ImportExcel(IDTO_GRPC<IDTO_Import> input);

        UnaryResult<MagicOnionResult<byte[]>> ExportExcel(IDTO_GRPC<IDTO_Export> input);
    }
}
