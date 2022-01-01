namespace Spear.MidM.Attachment
{
    public class AttachmentResult
    {
        public Enum_AttachmentResult State { get; set; } = Enum_AttachmentResult.None;

        public string FilePath { get; set; } = "";
    }
}
