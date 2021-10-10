using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Spear.Inf.Core.Base;
using Spear.Inf.Core.CusResult;
using Spear.Demo4WebApi.Contract.DTO.Input;
using Spear.Demo4WebApi.Contract.Interface;
using Spear.Demo4WebApi.Contract.DTO.Ouput;

namespace Spear.Demo4WebApi.Host
{
    [Route("order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        public IWebApiService _webapiService;

        public OrderController(IWebApiService webapiService)
        {
            _webapiService = webapiService;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("page")]
        public async Task<ResultWebApi<ODTO_CommonOrder>> Page(IDTO_CommonOrder input)
        {
            var result = await _webapiService.Page(input);
            return result.Success();
        }
    }
}
