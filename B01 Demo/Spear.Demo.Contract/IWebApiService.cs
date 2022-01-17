using System.Threading.Tasks;

using Spear.Demo.Inf.DTO;
using Spear.Inf.Core.ServGeneric.IOC;

namespace Spear.Demo.Contract
{
    public interface IWebApiService : ITransient
    {
        Task<ODTO_CommonOrder> Page(IDTO_CommonOrder input);
    }
}
