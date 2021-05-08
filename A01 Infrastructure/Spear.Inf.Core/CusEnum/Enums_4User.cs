using Spear.Inf.Core.Attr;

namespace Spear.Inf.Core.CusEnum
{
    public enum Enum_Role
    {
        [Remark("默认、无")]
        None,

        [Remark("管理员")]
        Admin,

        [Remark("用户")]
        User,

        [Remark("超级管理员")]
        SuperAdmin,

        [Remark("普通管理员")]
        NormalAdmin,
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
