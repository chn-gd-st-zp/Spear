using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;

namespace Spear.Inf.Core
{
    public class SpearHostedService : IHostedService
    {
        protected ISpearRunner Runner;

        protected BackgroundWorker BackgroundWorker { get; private set; }

        public SpearHostedService(ISpearRunner runner)
        {
            Runner = runner;
            BackgroundWorker = new BackgroundWorker();
            BackgroundWorker.DoWork += (object sender, DoWorkEventArgs e) => { Runner.Run((CancellationToken)e.Argument); };
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            BackgroundWorker.RunWorkerAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Runner.Dispose(cancellationToken);
            BackgroundWorker.Dispose();
        }
    }

    public class SpearHostedService<TSpearRunner> : SpearHostedService
        where TSpearRunner : ISpearRunner
    {
        public SpearHostedService(TSpearRunner runner) : base(runner) { }
    }
}
