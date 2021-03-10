using System;
using System.Collections.Generic;

namespace FlmAutoRent.Data.Entities
{
    public class ProfilingOperator
    {
        public ProfilingOperator() {
            SystemsLogs = new HashSet<SystemLog>();
            ProfilingOperatorGroups = new HashSet<ProfilingOperatorGroup>();
            ProfilingOperatorPasswordHistories = new HashSet<ProfilingOperatorPasswordHistory>();
            ProfilingOperatorEmails = new HashSet<ProfilingOperatorEmail>();
        }

        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Token { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string UserId { get; set;}
        public string Email { get; set; }
        public string Password { get; set; }
        public int PasswordDeadline { get; set; }
        public DateTime PasswordLastEdit { get; set; }
        public string PhoneNr { get; set; }
        public int Enabled { get; set; }
        public DateTime OperatorData{ get; set;}
        public string Avatar { get; set; }

        public ICollection<SystemLog> SystemsLogs { get; set; }
        public ICollection<ProfilingOperatorGroup> ProfilingOperatorGroups { get; set; } 
        public ICollection<ProfilingOperatorPasswordHistory> ProfilingOperatorPasswordHistories { get; set; } 
        public ICollection<ProfilingOperatorEmail> ProfilingOperatorEmails { get; set; } 

    }
}