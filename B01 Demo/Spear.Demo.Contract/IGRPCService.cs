using System.Collections.Generic;

using Spear.Inf.Core.CusResult;
using Spear.Inf.Core.DTO;
using Spear.Inf.Core.Injection;

using Spear.Demo.Inf.DTO;

namespace Spear.Demo.Contract
{
    public interface IGRPCService : ITransient
    {
        ResultBase<List<ODTOTestDemo>> List(IDTO_ListParam input);

        ResultBase<ODTO_Page<ODTOTestDemo>> Page(IDTO_PageParam input);

        ResultBase<ODTO_Tree<ODTOTestDemo>> Tree(IDTO_TreeParam input);

        ResultBase<List<ODTOTestDemo>> ImportExcel(IDTO_Import input);

        ResultBase<byte[]> ExportExcel(IDTO_Export input);
    }
}
