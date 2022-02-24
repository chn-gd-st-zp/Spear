using System;
using System.Threading.Tasks;

using Hangfire;
using Quartz;
using Quartz.Spi;

using Spear.Inf.Core;
using Spear.Inf.Core.Attr;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.Hosted;

namespace Spear.MidM.Schedule
{
    [DIModeForService(Enum_DIType.ExclusiveByKeyed, typeof(Schedule4Timer), Enum_ScheduleType.Timer)]
    public class Schedule4Timer : SpearScheduleBase
    {
        protected override void DoWork()
        {
            var settings = ServiceContext.Resolve<ScheduleSettings>();

            foreach (var timer in settings.Timers)
            {
                foreach (var item in timer.Items)
                {
                    var runner = ServiceContext.ResolveByNamed<IRunner4Timer>(timer.Type);
                    var register = ServiceContext.Resolve<IRegister4Timer>();
                    register.Regis(runner, timer.Cron, item);
                }
            }
        }
    }

    public class Register4HangFire : IRegister4Timer
    {
        public Task Regis(IRunner4Timer runner, string cron, TimerParam param)
        {
            RecurringJob.AddOrUpdate(param.Name, () => runner.Run(param.Name, param.Args), cron, TimeZoneInfo.Local);
            return Task.CompletedTask;
        }
    }

    public class Register4Quartz : IRegister4Timer
    {
        private IScheduler _scheduler { get; set; }

        public Register4Quartz()
        {
            _scheduler = ServiceContext.Resolve<ISchedulerFactory>().GetScheduler().Result;
            _scheduler.JobFactory = ServiceContext.Resolve<IJobFactory>();

            _scheduler.Start().Wait();
        }

        public async Task Regis(IRunner4Timer runner, string cron, TimerParam param)
        {
            var type = runner.GetType();

            var job = JobBuilder
                .Create(type)
                .WithIdentity(type.FullName)
                .WithDescription(param.Name)
                .Build();

            var trigger = TriggerBuilder
                .Create()
                .WithIdentity($"{type.FullName}.trigger")
                .WithCronSchedule(cron)
                .WithDescription(cron)
                .Build();

            await _scheduler.ScheduleJob(job, trigger);
        }
    }

    public class JobFactory : IJobFactory
    {
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var result = ServiceContext.ResolveByKeyed<IJob>(bundle.JobDetail.JobType);
            return result;
        }

        public void ReturnJob(IJob job) { }
    }
}
