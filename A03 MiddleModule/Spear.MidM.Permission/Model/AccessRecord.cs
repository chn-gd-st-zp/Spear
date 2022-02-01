﻿using System.Collections.Generic;

using Spear.Inf.Core.CusEnum;

namespace Spear.MidM.Permission
{
    public class AccessRecord
    {
        public Enum_Role ERoleType { get; set; }

        public string AccountID { get; set; }

        public string UserName { get; set; }

        public Enum_OperationType EOperationType { get; set; }

        public string ModuleName { get; set; }

        public string DBTableName { get; set; }

        public string DataPrimeryKey { get; set; }

        public List<AccessRecordDescription> Descriptions { get; set; }

        public object ExecResult { get; set; }
    }

    public class AccessRecordDescription
    {
        public string FieldName { get; set; }

        public string FieldRemark { get; set; }

        public object InputValue { get; set; }

        public object DBValue { get; set; }
    }
}