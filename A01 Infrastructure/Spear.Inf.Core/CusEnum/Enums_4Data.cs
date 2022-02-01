using Spear.Inf.Core.Attr;

namespace Spear.Inf.Core.CusEnum
{
    [KVResource]
    public enum Enum_Status
    {
        [Remark("默认、无")]
        None,

        [Remark("正常")]
        Normal,

        [Remark("禁用")]
        Disable,

        [Remark("删除")]
        Delete,
    }

    [KVResource]
    public enum Enum_Process
    {
        [Remark("默认、无")]
        None,

        [Remark("等待中")]
        Waiting,

        [Remark("处理中")]
        Processing,

        [Remark("重新处理中")]
        ReProcessing,

        [Remark("完成")]
        Finished,

        [Remark("待确认")]
        Confirming,

        [Remark("成功")]
        Success,

        [Remark("失败")]
        Fail,
    }

    [KVResource]
    public enum Enum_OperationType
    {
        [Remark("默认、无")]
        None,

        [Remark("新增")]
        Create,

        [Remark("删除")]
        Delete,

        [Remark("修改")]
        Update,

        [Remark("查询")]
        Search,
    }

    [KVResource]
    public enum Enum_TreeNodeType
    {
        [Remark("默认、无")]
        None,

        [Remark("组")]
        Group,

        [Remark("节点")]
        Node,
    }

    [KVResource]
    public enum Enum_PermissionType
    {
        [Remark("默认、无")]
        None,

        [Remark("组")]
        Group,

        [Remark("方法")]
        Action,

        [Remark("数据")]
        Data,
    }
}
