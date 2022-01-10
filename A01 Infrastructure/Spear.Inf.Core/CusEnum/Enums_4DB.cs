using Spear.Inf.Core.Attr;

namespace Spear.Inf.Core.CusEnum
{
    public enum Enum_DBType
    {
        [Remark("默认、无")]
        None,

        [Remark("MSSQL")]
        MSSQL,

        [Remark("MySQL")]
        MySQL,
    }

    public enum Enum_ORMType
    {
        [Remark("默认、无")]
        None,

        [Remark("EF")]
        EF,

        [Remark("SqlSugar")]
        SqlSugar,
    }
}
