using System.Collections.Generic;

using Spear.Inf.Core.CusResult;
using Spear.Inf.Core.ServGeneric.IOC;
using Spear.Demo4GRPC.Pub.TestDemo;

namespace Spear.Demo4GRPC.Host.Server.Contract
{
    public interface IFileHandle : ISingleton
    {
        ResultBase<List<ODTOTestDemo>> ImportExcel(ImportParam input);

        ResultBase<byte[]> ExportExcel(ExportParam input);
    }
}
