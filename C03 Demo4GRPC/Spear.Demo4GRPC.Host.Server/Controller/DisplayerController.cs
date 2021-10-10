using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Spear.Inf.Core.Base;
using Spear.Inf.Core.CusResult;
using Spear.Inf.Core.DTO;
using Spear.Inf.Core.ServGeneric;
using Spear.Inf.Core.Tool;
using Spear.Demo4GRPC.Pub.Base;
using Spear.Demo4GRPC.Pub.TestDemo;

namespace Spear.Demo4GRPC.Host.Server.Controller
{
    [Route("displayer")]
    [ApiController]
    public class DisplayerController : ControllerBase
    {
        private readonly IDisplayer_TextDemo _displayer;

        public DisplayerController()
        {
            _displayer = ServiceContext.Resolve<IDataProvider>().Displayer as IDisplayer_TextDemo;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("list")]
        public async Task<ResultWebApi<List<ODTOTestDemo>>> List(ListParam input)
        {
            var result = _displayer.List(input);

            return result.ToResultWebApi();
        }

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("page")]
        public async Task<ResultWebApi<ODTO_Page<ODTOTestDemo>>> Page(PageParam input)
        {
            var result = _displayer.Page(input);

            return result.ToResultWebApi();
        }

        /// <summary>
        /// 获取树形
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("tree")]
        public async Task<ResultWebApi<ODTO_Tree<ODTOTestDemo>>> Tree(TreeParam input)
        {
            var result = _displayer.Tree(input);

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
            ImportParam inputParam = new ImportParam()
            {
                FileBase64 = input.OpenReadStream().Convert2Base64(true)
            };

            var result = _displayer.ImportExcel(inputParam);

            return result.ToResultWebApi();
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("export")]
        public IActionResult ExportExcel(ExportParam input)
        {
            var result = _displayer.ExportExcel(input);
            if (result.IsSuccess)
                return File(result.Data, "application/octet-stream", DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls");

            return result.ToResultWebApi().ToJsonResult();
        }
    }
}
