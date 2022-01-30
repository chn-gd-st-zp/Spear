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
        Task<ProcessResult<List<ODTOTestDemo>>> List(IDTO_ListParam input);

        Task<ProcessResult<ODTO_Page<ODTOTestDemo>>> Page(IDTO_PageParam input);

        Task<ProcessResult<ODTO_Tree<ODTOTestDemo>>> Tree(IDTO_TreeParam input);

        Task<ProcessResult<List<ODTOTestDemo>>> ImportExcel(IDTO_Import input);

        Task<ProcessResult<byte[]>> ExportExcel(IDTO_Export input);
    }
}
