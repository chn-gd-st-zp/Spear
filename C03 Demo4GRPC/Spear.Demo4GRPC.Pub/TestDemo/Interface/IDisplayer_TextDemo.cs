using System.Collections.Generic;

using Spear.Demo4GRPC.Pub.Base;
using Spear.Inf.Core.CusResult;
using Spear.Inf.Core.DTO;

namespace Spear.Demo4GRPC.Pub.TestDemo
{
    public interface IDisplayer_TextDemo : IDisplayer
    {
        ResultBase<List<ODTOTestDemo>> List(ListParam input);

        ResultBase<ODTO_Page<ODTOTestDemo>> Page(PageParam input);

        ResultBase<ODTO_Tree<ODTOTestDemo>> Tree(TreeParam input);

        ResultBase<List<ODTOTestDemo>> ImportExcel(ImportParam input);

        ResultBase<byte[]> ExportExcel(ExportParam input);
    }
}
