using System.Collections.Generic;

using Spear.Demo4GRPC.Pub.Basic;
using Spear.Inf.Core.CusResult;
using Spear.Inf.Core.DTO;

namespace Spear.Demo4GRPC.Pub.TestDemo
{
    public interface IDisplayer_TextDemo : IDisplayer
    {
        ResultBasic<List<ODTOTestDemo>> List(ListParam input);

        ResultBasic<ODTO_Page<ODTOTestDemo>> Page(PageParam input);

        ResultBasic<ODTO_Tree<ODTOTestDemo>> Tree(TreeParam input);

        ResultBasic<List<ODTOTestDemo>> ImportExcel(ImportParam input);

        ResultBasic<byte[]> ExportExcel(ExportParam input);
    }
}
