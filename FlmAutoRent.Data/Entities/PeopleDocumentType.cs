using System;
using System.Collections.Generic;

namespace FlmAutoRent.Data.Entities
{
    public partial class PeopleDocumentType
    {
        public PeopleDocumentType(){
            PeopleDocuments = new HashSet<PeopleDocument>();
        }

        public int Id { get; set; }
        public string DocumentType { get; set; }

        
        public ICollection<PeopleDocument> PeopleDocuments{ get; set; } 

    }
}