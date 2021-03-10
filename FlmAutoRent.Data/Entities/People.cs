using System;
using System.Collections.Generic;

namespace FlmAutoRent.Data.Entities
{
    public partial class People
    {
        public People(){
            PeopleDocuments = new HashSet<PeopleDocument>();
            PeopleMessages = new HashSet<PeopleMessage>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string PhoneNr { get; set; }

        public ICollection<PeopleDocument> PeopleDocuments { get; set; } 
        public ICollection<PeopleMessage> PeopleMessages { get; set; }
    }
}