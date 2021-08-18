using System;
using System.Threading.Tasks;

using Quartz;

using Spear.Inf.Core.Interface;
using Spear.Inf.Core.ServGeneric;
using Spear.Inf.Core.Tool;

namespace Spear.MidM.Quartz
{
    [DisallowConcurrentExecution]
    public abstract class QuartzJobBase : IJob
    {
        protected abstract string JobName { get; }

        protected readonly ILogger Logger;

        public QuartzJobBase()
        {
            Logger = ServiceContext.Resolve<ILogger>();
        }

        public async Task Execute(IJobExecutionContext context)
        {
            Info($"{DateTime.Now.ToDateTimeString()} - 日程[{JobName}] - 启动");

            try
            {
                Execute();
            }
            catch (Exception ex)
            {
                Error($"{DateTime.Now.ToDateTimeString()} - 日程[{JobName}] - 出错：{ex.Message}", ex);
            }
        }

        protected void Info(string text)
        {
            Printor.PrintText(text);

            if (Logger != null)
                Logger.Info(text);
        }

        protected void Error(string text, Exception ex)
        {
            Printor.PrintText(text);

            if (Logger != null)
                Logger.Error(text, ex);
        }

        public abstract void Execute();
    }

    [DisallowConcurrentExecution]
    public abstract class JobBase<TJob> : QuartzJobBase
        where TJob : class
    {
        protected new readonly ILogger<TJob> Logger;
    }
}
