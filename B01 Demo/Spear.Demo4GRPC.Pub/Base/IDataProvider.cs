using Spear.Inf.Core.ServGeneric.IOC;

namespace Spear.Demo4GRPC.Pub.Base
{
    public interface IDataProvider : ITransient
    {
        ISpider Spider { get; }

        IAnalyzer Analyzer { get; }

        IDisplayer Displayer { get; }
    }
}
