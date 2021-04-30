using Spear.Inf.Core.CusResult;
using Spear.Inf.Core.ServGeneric.IOC;

namespace Spear.Demo4GRPC.Pub.Basic
{
    public interface IAnalyzer : ITransient
    {
        ResultBasic<bool> Run();
    }
}
