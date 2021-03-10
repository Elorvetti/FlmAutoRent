using System;
using System.Collections.Generic;

namespace FlmAutoRent.Data.Entities
{
    public class SystemEmail
    {
        public SystemEmail(){
            SystemEmailMessages = new HashSet<SystemEmailMessage>();
            SystemEmailAttachments = new HashSet<SystemEmailAttachment>();
            ProfilingOperatorEmails = new HashSet<ProfilingOperatorEmail>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string EmailPop { get; set; }
        public string EmailSmtp { get; set; }
        public int EmailPortSmtp { get; set; }
        public int EmailPortPop { get; set; }
        public int EmailSendUsing { get; set; }
        public string EmailUser { get; set; }
        public string EmailPassword { get; set; }
        public int EmailSSL { get; set; }
        public int EmailAuthenticated { get; set; }
        public string EmailEnable { get; set; }
        public string EmailDelete { get; set; }
        public string EmailManage { get; set; }
        public Guid EmailGuid { get; set; }
        public string EmailDefault { get; set; }
        public string EmailBccDefault { get; set; }
        public string EmailSignature { get; set; }

        public ICollection<SystemEmailMessage> SystemEmailMessages { get; set; }
        public ICollection<SystemEmailAttachment> SystemEmailAttachments { get; set; }
        public ICollection<ProfilingOperatorEmail> ProfilingOperatorEmails { get; set; }
    }
}