using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Spear.Inf.Core;
using Spear.Inf.Core.Base;
using Spear.Inf.Core.CusResult;
using Spear.Inf.Core.DTO;
using Spear.Inf.Core.Tool;

using Spear.Demo.Contract;
using Spear.Demo.Inf.DTO;

namespace Spear.Demo4GRPC.Host.Server.Controller
{
    [ApiVersion("1.0", Deprecated = true)]
    [ApiController, Route("api/[controller]"), Route("api/v{version:apiVersion}/[controller]")]
    public class GRPCDemoController : ControllerBase
    {
        private readonly IGRPCService _grpcService;

        public GRPCDemoController()
        {
            _grpcService = ServiceContext.Resolve<IGRPCService>();
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("List")]
        public async Task<WebApiResult<List<ODTOTestDemo>>> List(IDTO_ListParam input)
        {
            var result = await _grpcService.List(input);

            return result.ToAPIResult();
        }

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("Page")]
        public async Task<WebApiResult<ODTO_Page<ODTOTestDemo>>> Page(IDTO_PageParam input)
        {
            var result = await _grpcService.Page(input);

            return result.ToAPIResult();
        }

        /// <summary>
        /// 获取树形
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("Tree")]
        public async Task<WebApiResult<ODTO_Tree<ODTOTestDemo>>> Tree(IDTO_TreeParam input)
        {
            var result = await _grpcService.Tree(input);

            return result.ToAPIResult();
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("ImportExcel")]
        public async Task<WebApiResult<List<ODTOTestDemo>>> ImportExcel(IFormFile input)
        {
            IDTO_Import inputParam = new IDTO_Import()
            {
                FileBase64 = input.OpenReadStream().ToBase64(true)
            };

            var result = await _grpcService.ImportExcel(inputParam);

            return result.ToAPIResult();
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("ExportExcel")]
        public IActionResult ExportExcel(IDTO_Export input)
        {
            var result = _grpcService.ExportExcel(input).Result;
            if (result.IsSuccess)
                return File(result.Data, "application/octet-stream", DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls");

            return result.ToAPIResult().ToJsonResult();
        }
    }
}
