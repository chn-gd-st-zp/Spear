using System.Collections.Generic;

using Spear.Inf.Core.CusResult;
using Spear.Inf.Core.ServGeneric.IOC;
using Spear.Demo4GRPC.Pub.TestDemo;

namespace Spear.Demo4GRPC.Host.Server.Contract
{
    public interface IFileHandle : ISingleton
    {
        ResultBasic<List<ODTOTestDemo>> ImportExcel(ImportParam input);

        ResultBasic<byte[]> ExportExcel(ExportParam input);
    }
}
