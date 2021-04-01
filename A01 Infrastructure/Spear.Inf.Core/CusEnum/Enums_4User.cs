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

        [Remark("代理管理员")]
        AgentAdmin,

        [Remark("普通用户")]
        NormalUser,
    }

    public enum Enum_UserType
    {
        [Remark("默认、无")]
        None = 0,

        [Remark("前端用户")]
        FrontendUser = 1,

        [Remark("后端用户")]
        BackendUser = 2
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
