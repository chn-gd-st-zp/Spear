using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;

namespace Spear.Inf.Core
{
    public abstract class HostedServiceBase : IHostedService
    {
        protected IRunner Runner;
        protected BackgroundWorker BackgroundWorker { get; private set; }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            BackgroundWorker = new BackgroundWorker();
            BackgroundWorker.DoWork += _backgroundWorker_DoWork;

            BackgroundWorker.RunWorkerAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Runner.Dispose(cancellationToken);
            BackgroundWorker.Dispose();
        }

        private void _backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Runner.Run((CancellationToken)e.Argument);
        }
    }

    public class HostedService : HostedServiceBase
    {
        public HostedService(IRunner runner)
        {
            Runner = runner;
        }
    }

    public class HostedService<TRunner> : HostedServiceBase
        where TRunner : IRunner
    {
        public HostedService(TRunner runner)
        {
            Runner = runner;
        }
    }
}
