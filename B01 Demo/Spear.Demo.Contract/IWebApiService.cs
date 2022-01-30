using System.Threading.Tasks;

using Spear.Inf.Core.CusResult;
using Spear.Inf.Core.Injection;

using Spear.Demo.Inf.DTO;

namespace Spear.Demo.Contract
{
    public interface IWebApiService : ITransient
    {
        Task<ProcessResult<ODTO_CommonOrder>> Page(IDTO_CommonOrder input);
    }
}
