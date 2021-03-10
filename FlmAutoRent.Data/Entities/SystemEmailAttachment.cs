namespace FlmAutoRent.Data.Entities
{
    public class SystemEmailAttachment
    {
        public int Id { get; set; }
        public string AttachmentName { get; set; }
        public string DirectionAttachments { get; set; }

        public SystemEmail SystemEmails { get; set; }
    }
}