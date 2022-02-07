using System.Collections.Generic;

using Spear.Inf.Core.CusEnum;

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
}
