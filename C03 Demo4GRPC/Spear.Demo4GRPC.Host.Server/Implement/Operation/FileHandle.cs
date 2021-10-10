using System.Collections.Generic;
using System.Data;

using AutoMapper;

using Spear.Inf.Core.CusResult;
using Spear.Inf.Core.ServGeneric;
using Spear.Inf.Core.Tool;
using Spear.Inf.DataFile.Excel;
using Spear.Demo4GRPC.Pub.TestDemo;
using Spear.Demo4GRPC.Host.Server.Contract;

namespace Spear.Demo4GRPC.Host.Server.Implement
{
    public class FileHandle : IFileHandle
    {
        private readonly IMapper _mapper;
        private readonly IExcelHelper _excelHelper;

        public FileHandle()
        {
            _mapper = ServiceContext.Resolve<IMapper>();
            _excelHelper = ServiceContext.Resolve<IExcelHelper>();
        }

        public ResultBase<List<ODTOTestDemo>> ImportExcel(ImportParam input)
        {
            List<ODTOTestDemo> result = new List<ODTOTestDemo>();

            using (var stream = input.FileBase64.Convert2Stream())
            {
                List<ImportTestDemo> importData = stream.ToObject<ImportTestDemo>();
                importData.ForEach(o => { result.Add(_mapper.Map<ODTOTestDemo>(o)); });
            }

            return result.ResultBase_Success();
        }

        public ResultBase<byte[]> ExportExcel(ExportParam input)
        {
            var dataList = new List<ExportSetTestDemo>();
            for (int i = 0; i < 10; i++)
            {
                dataList.Add(new ExportSetTestDemo
                {
                    Amt = 1000,
                    Use = 200,
                    Rate = DataConvert.Divider(200, 1000),
                });
            }

            DataTable dt = dataList.ToDataTable();
            var execResult = _excelHelper.ExportFromDataTable(dt);

            return execResult.ResultBase_Success();
        }
    }
}
