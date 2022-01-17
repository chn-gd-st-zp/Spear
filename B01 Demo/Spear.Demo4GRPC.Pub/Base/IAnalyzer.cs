using Spear.Inf.Core.CusResult;
using Spear.Inf.Core.ServGeneric.IOC;

namespace Spear.Demo4GRPC.Pub.Base
{
    public interface IAnalyzer : ITransient
    {
        ResultBase<bool> Run();
    }
}
