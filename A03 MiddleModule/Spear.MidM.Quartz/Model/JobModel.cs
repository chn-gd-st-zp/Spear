using System;

using Quartz;
using Quartz.Spi;

using Spear.Inf.Core.ServGeneric;

namespace Spear.MidM.Quartz
{
    [AttributeUsage(AttributeTargets.Class)]
    public class JobAttribute : Attribute
    {
        public readonly string Name;

        public JobAttribute(string name) { Name = name; }
    }

    public class JobDetail
    {
        public Type Type { get; }

        public string Cron { get; }

        public JobDetail(Type type, string cron)
        {
            Type = type;
            Cron = cron;
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
