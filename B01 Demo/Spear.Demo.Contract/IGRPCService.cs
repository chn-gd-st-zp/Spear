using System.Collections.Generic;
using System.Threading.Tasks;

using Spear.Inf.Core.CusResult;
using Spear.Inf.Core.DTO;
using Spear.Inf.Core.Injection;

using Spear.Demo.Inf.DTO;

namespace Spear.Demo.Contract
{
    public interface IGRPCService : ITransient
    {
        Task<ResultBase<List<ODTOTestDemo>>> List(IDTO_ListParam input);

        Task<ResultBase<ODTO_Page<ODTOTestDemo>>> Page(IDTO_PageParam input);

        Task<ResultBase<ODTO_Tree<ODTOTestDemo>>> Tree(IDTO_TreeParam input);

        Task<ResultBase<List<ODTOTestDemo>>> ImportExcel(IDTO_Import input);

        Task<ResultBase<byte[]>> ExportExcel(IDTO_Export input);
    }
}
