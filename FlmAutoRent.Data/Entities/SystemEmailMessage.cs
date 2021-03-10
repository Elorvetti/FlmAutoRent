using System;

namespace FlmAutoRent.Data.Entities
{
    public class SystemEmailMessage
    {
        public int Id { get; set; }
        public string EmailGuid { get; set; }
        public DateTime EmailData { get; set; }
        public string EmailFrom { get; set; }
        public string EmailTo { get; set; }
        public string EmailCC { get; set; }
        public string EmailBcc { get; set; }
        public string EmailObject{ get; set; }
        public string EmailMessage { get; set; }
        public string EmailRead { get; set; }
        public string InOut { get; set; }
        public int EmailAttachments { get; set; }

        public SystemEmail SystemEmails { get; set; }
    }
}