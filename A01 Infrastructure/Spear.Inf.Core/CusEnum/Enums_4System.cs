using Spear.Inf.Core.Attr;

namespace Spear.Inf.Core.CusEnum
{
    public enum Enum_Environment
    {
        [Remark("默认、无")]
        None,

        [Remark("开发环境")]
        Development,

        [Remark("测试环境")]
        Staging,

        [Remark("生产环境")]
        Production,
    }

    public enum Enum_InitFile
    {
        [Remark("默认、无")]
        None,

        [Remark("DLL")]
        DLL,

        [Remark("XML")]
        XML,
    }
    public enum Enum_PathMode
    {
        [Remark("默认、无")]
        None = 0,

        [Remark("绝对路径")]
        ABS,

        [Remark("相对路径")]
        REF
    }

    public enum Enum_DIType
    {
        [Remark("无")]
        None,

        [Remark("自身")]
        AsSelf,

        [Remark("自动接口")]
        AsImpl,

        [Remark("指定对象")]
        Exclusive,

        [Remark("指定对象-ByNamed")]
        ExclusiveByNamed,

        [Remark("指定对象-ByKeyed")]
        ExclusiveByKeyed,
    }

    public enum Enum_DIKeyedNamedFrom
    {
        [Remark("无")]
        None,

        [Remark("从属性来")]
        FromProperty,
    }

    public enum Enum_FilterType
    {
        [Remark("默认、无")]
        None,

        [Remark("控制器")]
        Ctrler,

        [Remark("GRPC")]
        GRPC,
    }

    public enum Enum_ServType
    {
        [Remark("默认、无")]
        None,

        [Remark("WebApi")]
        WebApi,

        [Remark("GRPC")]
        GRPC,
    }

    public enum Enum_RemoteChannel
    {
        [Remark("默认、无")]
        None,

        [Remark("手机")]
        Mobile,

        [Remark("邮箱")]
        Email,
    }

    public enum Enum_TipsResult
    {
        [Remark("默认、无")]
        None,

        [Remark("确定")]
        OK,

        [Remark("取消")]
        Cancel,
    }

    public enum Enum_PermissionType
    {
        [Remark("无操作")]
        None,

        [Remark("组")]
        Group,

        [Remark("方法")]
        Action,

        [Remark("数据")]
        Data,
    }

    public enum Enum_TreeNodeType
    {
        [Remark("无操作")]
        None,

        [Remark("组")]
        Group,

        [Remark("方法")]
        Node,

    }

    public enum Enum_OperationType
    {
        [Remark("无操作")]
        None,

        [Remark("新增")]
        Create,

        [Remark("删除")]
        Delete,

        [Remark("修改")]
        Update,
    }
}
