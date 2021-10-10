using System.Collections.Generic;

using Spear.Inf.Core.CusResult;
using Spear.Inf.Core.DTO;
using Spear.Inf.Core.ServGeneric;
using Spear.Demo4GRPC.Pub.TestDemo;
using Spear.Demo4GRPC.Host.Server.Contract;

namespace Spear.Demo4GRPC.Host.Server.Implement
{
    public class Displayer : IDisplayer_TextDemo
    {
        private readonly IShow _show;
        private readonly IFileHandle _export;

        public Displayer()
        {
            _show = ServiceContext.Resolve<IShow>();
            _export = ServiceContext.Resolve<IFileHandle>();
        }

        public ResultBase<List<ODTOTestDemo>> List(ListParam input)
        {
            return _show.List(input);
        }

        public ResultBase<ODTO_Page<ODTOTestDemo>> Page(PageParam input)
        {
            return _show.Page(input);
        }

        public ResultBase<ODTO_Tree<ODTOTestDemo>> Tree(TreeParam input)
        {
            return _show.Tree(input);
        }

        public ResultBase<List<ODTOTestDemo>> ImportExcel(ImportParam input)
        {
            return _export.ImportExcel(input);
        }

        public ResultBase<byte[]> ExportExcel(ExportParam input)
        {
            return _export.ExportExcel(input);
        }
    }
}
