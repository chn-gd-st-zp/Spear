using System;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;

using Spear.Inf.Core;
using Spear.Inf.Core.Attr;
using Spear.Inf.Core.Base;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.CusResult;
using Spear.Inf.Core.DBRef;
using Spear.Inf.Core.DTO;
using Spear.Inf.Core.Tool;
using Spear.Inf.DataFile.Excel;

using Spear.Demo.Inf.DTO;
using Spear.Demo.Inf.Model;
using Spear.Demo.Contract;

namespace Spear.Demo4GRPC.Host.Server.Implement
{
    [DIModeForService(Enum_DIType.Exclusive, typeof(IGRPCService))]
    public class GRPCService : ServiceBase<GRPCService>, IGRPCService
    {
        private readonly IMapper _mapper;
        private readonly IExcelHelper _excelHelper;

        public GRPCService()
        {
            _mapper = ServiceContext.Resolve<IMapper>();
            _excelHelper = ServiceContext.Resolve<IExcelHelper>();
        }

        public async Task<ProcessResult<List<ODTOTestDemo>>> List(IDTO_ListParam input)
        {
            var dataList = new List<ODTOTestDemo>();
            return dataList.ToServSuccess();
        }

        public async Task<ProcessResult<ODTO_Page<ODTOTestDemo>>> Page(IDTO_PageParam input)
        {
            var pageData = new Tuple<List<ODTOTestDemo>, int>(new List<ODTOTestDemo>(), 0);
            var dataList = pageData.ToODTOPage(input);

            return dataList.ToServSuccess();
        }

        public async Task<ProcessResult<ODTO_Tree<ODTOTestDemo>>> Tree(IDTO_TreeParam input)
        {
            var dataList = new List<ODTOTestDemo>();

            return dataList.ToTree("").ToServSuccess();
        }

        public async Task<ProcessResult<List<ODTOTestDemo>>> ImportExcel(IDTO_Import input)
        {
            List<ODTOTestDemo> result = new List<ODTOTestDemo>();

            using (var stream = input.FileBase64.ToStream())
            {
                List<MImport> importData = stream.ToObject<MImport>();
                importData.ForEach(o => { result.Add(_mapper.Map<ODTOTestDemo>(o)); });
            }

            return result.ToServSuccess();
        }

        public async Task<ProcessResult<byte[]>> ExportExcel(IDTO_Export input)
        {
            var dataList = new List<MExport>();
            for (int i = 0; i < 10; i++)
            {
                dataList.Add(new MExport
                {
                    Amt = 1000,
                    Use = 200,
                    Rate = MathConverter.Divider(200, 1000),
                });
            }

            DataTable dt = dataList.ToDataTable();
            var execResult = _excelHelper.ExportFromDataTable(dt);

            return execResult.ToServSuccess();
        }
    }
}
