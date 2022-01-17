using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Spear.Inf.Core.CusResult;
using Spear.Inf.Core.DTO;
using Spear.Inf.Core.ServGeneric;
using Spear.Inf.Core.Tool;

using Spear.Demo.Contract;
using Spear.Demo.Inf.DTO;

namespace Spear.Demo4GRPC.Host.Server.Controller
{
    [Route("grpc")]
    [ApiController]
    public class GRPCController : ControllerBase
    {
        private readonly IGRPCService _grpcService;

        public GRPCController()
        {
            _grpcService = ServiceContext.Resolve<IGRPCService>();
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("list")]
        public async Task<ResultWebApi<List<ODTOTestDemo>>> List(IDTO_ListParam input)
        {
            var result = _grpcService.List(input);

            return result.ToResultWebApi();
        }

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("page")]
        public async Task<ResultWebApi<ODTO_Page<ODTOTestDemo>>> Page(IDTO_PageParam input)
        {
            var result = _grpcService.Page(input);

            return result.ToResultWebApi();
        }

        /// <summary>
        /// 获取树形
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("tree")]
        public async Task<ResultWebApi<ODTO_Tree<ODTOTestDemo>>> Tree(IDTO_TreeParam input)
        {
            var result = _grpcService.Tree(input);

            return result.ToResultWebApi();
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("import")]
        public async Task<ResultWebApi<List<ODTOTestDemo>>> ImportExport(IFormFile input)
        {
            IDTO_Import inputParam = new IDTO_Import()
            {
                FileBase64 = input.OpenReadStream().Convert2Base64(true)
            };

            var result = _grpcService.ImportExcel(inputParam);

            return result.ToResultWebApi();
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("export")]
        public IActionResult ExportExcel(IDTO_Export input)
        {
            var result = _grpcService.ExportExcel(input);
            if (result.IsSuccess)
                return File(result.Data, "application/octet-stream", DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls");

            return result.ToResultWebApi().ToJsonResult();
        }
    }
}
