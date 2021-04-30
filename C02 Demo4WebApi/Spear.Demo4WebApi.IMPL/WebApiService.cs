using System.Threading.Tasks;

using Spear.Inf.Core.ServGeneric;
using Spear.Demo4WebApi.Contract.DTO.Input;
using Spear.Demo4WebApi.Contract.DTO.Ouput;
using Spear.Demo4WebApi.Contract.Interface;
using Spear.Demo4WebApi.Repository;

namespace Spear.Demo4WebApi.IMPL
{
    public class WebApiService : IWebApiService
    {
        public async Task<ODTO_CommonOrder> Page(IDTO_CommonOrder input)
        {
            var rp = ServiceContext.ResolveByKeyed<EFRepository_CommonOrder>(input.EUserType);
            var execResult = rp.Page(input.BeginTime, input.EndTime, input);
            var result = new ODTO_CommonOrder { EUserType = input.EUserType, Result = execResult };
            return result;
        }
    }
}
