using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Spear.Inf.Core.Base;
using Spear.Inf.Core.CusResult;

using Spear.Demo.Contract;
using Spear.Demo.Inf.DTO;

namespace Spear.Demo4WebApi.Host.Controller.v1
{
    [ApiVersion("1.0", Deprecated = true)]
    [ApiVersion("2.0", Deprecated = false)]
    [ApiVersion("3.0", Deprecated = false)]
    [ApiController, Route("api/[controller]"), Route("api/v{version:apiVersion}/[controller]")]
    public class OrderController : ControllerBase
    {
        public IWebApiService _webapiService;

        public OrderController(IWebApiService webapiService)
        {
            _webapiService = webapiService;
        }

        /// <summary>
        /// 测试
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("Test")]
        public async Task<ResultWebApi<string>> Test()
        {
            var result = new ResultBase<string> { IsSuccess = true, Data = HttpContext.GetRequestedApiVersion().ToString() };
            return result.ToAPIResult();
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
            return result.ToAPIResult();
        }
    }
}
