using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.ServGeneric;
using Spear.MidM.Quartz;

namespace Spear.Demo4WinApp.Host.Job
{
    [Job("AutoDelJob")]
    public class AutoDelJob : QuartzJobBasic
    {
        protected override string JobName { get { return "AutoDelJob"; } }

        public override void Execute()
        {
            var a = ServiceContext.Resolve<HAHAHAHA_DB>();
            var b = ServiceContext.Resolve<HAHAHAHA_List>();
            var c = ServiceContext.ResolveByKeyed<HAHAHAHA_Item>("2");

            var autoDelSettings = ServiceContext.Resolve<AutoDelSettings>();

            foreach (var autoDelSetting in autoDelSettings)
            {
                var dayCycle = 0;

                switch (autoDelSetting.Expired.EType)
                {
                    case Enum_DateCycle.Day:
                        dayCycle = 1;
                        break;
                    case Enum_DateCycle.Week:
                        dayCycle = 7;
                        break;
                    case Enum_DateCycle.Month:
                        dayCycle = 30;
                        break;
                    case Enum_DateCycle.Season:
                        dayCycle = 90;
                        break;
                    case Enum_DateCycle.Year:
                        dayCycle = 365;
                        break;
                    default:
                        continue;
                }

                var fileAddrs = GetFileAddr(autoDelSetting.ABSPath, autoDelSetting.FileType);
                foreach (var fileAddr in fileAddrs)
                {
                    var fileInfo = new FileInfo(fileAddr);
                    var lastDateTime = fileInfo.LastWriteTime;
                    var timeSpan = DateTime.Now - lastDateTime;

                    if (timeSpan.TotalDays < dayCycle)
                        continue;

                    fileInfo.Delete();
                }
            }
        }

        private List<string> GetFileAddr(string path, string[] fileExts)
        {
            var result = new List<string>();

            if (!Directory.Exists(path))
                return result;

            var fileAddrs = new List<string>();

            fileAddrs = Directory.GetFiles(path).ToList();
            if (fileAddrs != null && fileAddrs.Count() != 0)
            {
                foreach (var fileExt in fileExts)
                    result.AddRange(fileAddrs.Where(o => o.EndsWith(fileExt, StringComparison.OrdinalIgnoreCase)).ToList());
            }

            fileAddrs = Directory.GetDirectories(path).ToList();
            if (fileAddrs != null && fileAddrs.Count() != 0)
            {
                foreach (var fileAddr in fileAddrs)
                    result.AddRange(GetFileAddr(fileAddr, fileExts));
            }

            return result;
        }
    }
}
