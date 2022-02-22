using System.Collections.Generic;
using System.Linq;

using Spear.Inf.Core;
using Spear.Inf.Core.Attr;
using Spear.Inf.Core.CusEnum;
using Spear.Inf.Core.Interface;
using Spear.Inf.Core.Tool;

namespace Spear.MidM.Permission
{
    public class AccessRecord
    {
        public Enum_Role ERoleType { get; set; }

        public string AccountID { get; set; }

        public string UserName { get; set; }

        public Enum_OperationType EOperationType { get; set; }

        public string TBName { get; set; }

        public string TBValue { get; set; }

        public string PKName { get; set; }

        public string PKValue { get; set; }

        public string TriggerName { get; set; }

        public List<AccessRecordDescription> Descriptions { get; set; }

        public object ExecResult { get; set; }
    }

    public class AccessRecordDescription
    {
        public virtual string FieldName { get; set; }

        public virtual string FieldRemark { get; set; }

        public virtual object InputValue { get; set; }

        public virtual object DBValue { get; set; }
    }

    public static class AccessRecordExtension
    {
        public static Dictionary<string, string> AccessRecordCategory()
        {
            var result = new Dictionary<string, string>();

            var dbContext = ServiceContext.Resolve<IDBContext>();

            AppInitHelper.GetAllType()
                .Where(o => o.IsClass && o.IsImplementedOf<IAccessRecordTrigger>())
                .ToList()
                .ForEach(o =>
                {
                    var key = dbContext.GetTBName(o);
                    var value = o.GetRemark();
                    result.Add(key, value);
                });

            return result;
        }
    }
}