using System.Threading.Tasks;

using Spear.Inf.Core.ServGeneric.IOC;
using Spear.Demo4WebApi.Contract.DTO.Input;
using Spear.Demo4WebApi.Contract.DTO.Ouput;

namespace Spear.Demo4WebApi.Contract.Interface
{
    public interface IWebApiService : ITransient
    {
        Task<ODTO_CommonOrder> Page(IDTO_CommonOrder input);
    }
}
