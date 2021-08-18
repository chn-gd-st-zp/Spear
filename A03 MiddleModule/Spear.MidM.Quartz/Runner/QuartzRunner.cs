using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Quartz;
using Quartz.Spi;

using Spear.Inf.Core;
using Spear.Inf.Core.ServGeneric;

namespace Spear.MidM.Quartz
{
    public class QuartzRunner : RunnerBase<QuartzRunner>
    {
        private IScheduler _scheduler { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        protected override void Init() { }

        /// <summary>
        /// 释放
        /// </summary>
        public override async Task Dispose(CancellationToken cancellationToken)
        {
            await _scheduler?.Shutdown(cancellationToken);
        }

        /// <summary>
        /// 执行
        /// </summary>
        protected override async Task Execute(CancellationToken cancellationToken)
        {
            try
            {
                _scheduler = await ServiceContext.Resolve<ISchedulerFactory>().GetScheduler(cancellationToken);
                _scheduler.JobFactory = ServiceContext.Resolve<IJobFactory>();

                var jobDetails = ServiceContext.Resolve<IEnumerable<JobDetail>>();
                foreach (var jobDetail in jobDetails)
                {
                    var jobType = jobDetail.Type;

                    try
                    {
                        var job = JobBuilder
                            .Create(jobType)
                            .WithIdentity(jobType.FullName)
                            .WithDescription(jobType.Name)
                            .Build();

                        var trigger = TriggerBuilder
                            .Create()
                            .WithIdentity($"{jobDetail.Type.FullName}.trigger")
                            .WithCronSchedule(jobDetail.Cron)
                            .WithDescription(jobDetail.Cron)
                            .Build();

                        await _scheduler.ScheduleJob(job, trigger, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error($"日程[{jobType.FullName}]执行出错：{ex.Message}", ex);
                        continue;
                    }
                }

                await _scheduler.Start(cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.Error($"日程服务运行出错：{ex.Message}", ex);
            }
        }
    }
}