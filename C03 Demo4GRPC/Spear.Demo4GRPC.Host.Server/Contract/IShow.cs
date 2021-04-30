using System.Collections.Generic;

using Spear.Inf.Core.CusResult;
using Spear.Inf.Core.DTO;
using Spear.Inf.Core.ServGeneric.IOC;
using Spear.Demo4GRPC.Pub.TestDemo;

namespace Spear.Demo4GRPC.Host.Server.Contract
{
    public interface IShow : ITransient
    {
        ResultBasic<List<ODTOTestDemo>> List(ListParam input);

        ResultBasic<ODTO_Page<ODTOTestDemo>> Page(PageParam input);

        ResultBasic<ODTO_Tree<ODTOTestDemo>> Tree(TreeParam input);
    }
}
