using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Autofac;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace Spear.MidM.Quartz
{
    public static class MidModule_Quartz
    {
        public static ContainerBuilder RegisQuartz(this ContainerBuilder containerBuilder, List<Type> runningTypeList, JobSettings jobSettings)
        {
            containerBuilder.RegisterType<StdSchedulerFactory>().As<ISchedulerFactory>().SingleInstance();
            containerBuilder.RegisterType<JobFactory>().As<IJobFactory>().SingleInstance();

            var objList = runningTypeList
                .Select(o => new { Type = o, Attr = o.GetCustomAttribute<JobAttribute>() })
                .Where(o => o.Attr != null)
                .ToList();

            foreach (var obj in objList)
            {
                var setting = jobSettings.Where(o => o.Name == obj.Attr.Name).SingleOrDefault();
                if (setting == null)
                    continue;

                containerBuilder.RegisterType(obj.Type).Keyed<IJob>(obj.Type).InstancePerDependency();
                containerBuilder.Register(o => new JobDetail(obj.Type, setting.Cron)).AsSelf().SingleInstance();
            }

            return containerBuilder;
        }
    }
}
