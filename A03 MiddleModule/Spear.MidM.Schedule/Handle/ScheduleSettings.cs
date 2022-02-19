using System.Collections.Generic;

using Spear.Inf.Core.Attr;
using Spear.Inf.Core.SettingsGeneric;

namespace Spear.MidM.Schedule
{
    public enum Enum_ScheduleType
    {
        [Remark("默认、无")]
        None = 0,

        [Remark("定时任务")]
        Timer,

        [Remark("后台任务")]
        BGWorker,
    }

    [DIModeForSettings("ScheduleSettings", typeof(ScheduleSettings))]
    public class ScheduleSettings : ISettings
    {
        public List<Enum_ScheduleType> RunningServices { get; set; }

        public List<TimerSettings> Timers { get; set; }

        public List<BGWorkerSettings> BGWorkers { get; set; }
    }

    public class ScheduleItemBase
    {
        public string Type { get; set; }
    }

    #region Timer

    public class TimerSettings : ScheduleItemBase
    {
        public string Cron { get; set; }

        public List<TimerParam> Items { get; set; }
    }

    public class TimerParam
    {
        public string Name { get; set; }

        public string[] Args { get; set; }
    }

    #endregion

    #region BGWorker

    public class BGWorkerSettings : ScheduleItemBase
    {
        public string Name { get; set; }

        public string[] Args { get; set; }
    }

    #endregion
}
