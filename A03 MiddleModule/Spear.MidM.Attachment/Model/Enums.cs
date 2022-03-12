using Spear.Inf.Core.Attr;

namespace Spear.MidM.Attachment
{
    public enum Enum_AttachmentHandler
    {
        [Remark("默认、无")]
        None = 0,

        [Remark("图片")]
        PIC,

        [Remark("文件")]
        DOC,

        [Remark("媒体")]
        Media,
    }

    public enum Enum_AttachmentPictureSize
    {
        [Remark("默认、无")]
        None = 0,

        [Remark("大")]
        Large,

        [Remark("中")]
        Medium,

        [Remark("小")]
        Small,
    }

    public enum Enum_AttachmentResult
    {
        [Remark("默认、无")]
        None = 0,

        [Remark("成功")]
        Success,

        [Remark("找不到相应的控制处理器")]
        HandlerNotFound,

        [Remark("找不到相应的流程处理方式")]
        OperationNotFound,

        [Remark("不支持该类型的后缀名")]
        ExtNotSupport,

        [Remark("文件大小超过限制")]
        OverSize,
    }
}
