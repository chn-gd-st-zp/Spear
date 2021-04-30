using System.Collections.Generic;

namespace Spear.MidM.Quartz
{
    public class JobSettings : List<JobSettingsItem>
    {
        //
    }

    public class JobSettingsItem
    {
        /// <summary>
        /// 作业名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// Cron 调度 表达式
        /// </summary>
        public string Cron { get; set; }
    }
}
