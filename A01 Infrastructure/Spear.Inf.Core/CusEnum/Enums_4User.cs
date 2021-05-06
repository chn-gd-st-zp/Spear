using Spear.Inf.Core.Attr;

namespace Spear.Inf.Core.CusEnum
{
    public enum Enum_Role
    {
        [Remark("默认、无")]
        None,

        [Remark("超级管理员")]
        SuperAdmin,

        [Remark("普通管理员")]
        NormalAdmin,

        [Remark("普通用户")]
        User,
    }

    public enum Enum_Gender
    {
        [Remark("默认、无")]
        None,

        [Remark("保密")]
        Secret,

        [Remark("男性")]
        Male,

        [Remark("女性")]
        Female,
    }
}
