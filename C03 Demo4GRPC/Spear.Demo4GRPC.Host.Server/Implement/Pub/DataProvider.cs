using Spear.Inf.Core.ServGeneric;
using Spear.Demo4GRPC.Pub.Basic;
using Spear.Demo4GRPC.Pub.TestDemo;

namespace Spear.Demo4GRPC.Host.Server.Implement
{
    public class DataProvider : IDataProvider_TestDemo
    {
        public DataProvider()
        {
            _spider = ServiceContext.Resolve<ISpider>();
            _analyzer = ServiceContext.Resolve<IAnalyzer>();
            _displayer = ServiceContext.Resolve<IDisplayer>();
        }

        private ISpider _spider;
        public ISpider Spider { get { return _spider; } }

        private IAnalyzer _analyzer;
        public IAnalyzer Analyzer { get { return _analyzer; } }

        private IDisplayer _displayer;
        public IDisplayer Displayer { get { return _displayer; } }
    }
}
