using Spear.Inf.Core;
using Spear.Inf.Core.Attr;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.ServGeneric;

namespace Spear.MidM.Schedule
{
    [DIModeForService(Enum_DIType.ExclusiveByKeyed, typeof(Schedule4BGWorker), Enum_ScheduleType.BGWorker)]
    public class Schedule4BGWorker : HSSpearScheduleBase
    {
        protected override void DoWork()
        {
            var settings = ServiceContext.Resolve<ScheduleSettings>();

            foreach (var bgWorker in settings.BGWorkers)
            {
                var runner = ServiceContext.ResolveByNamed<IRunner4BGWorker>(bgWorker.Type);
                runner.Run(bgWorker.Args);
            }
        }
    }
}
