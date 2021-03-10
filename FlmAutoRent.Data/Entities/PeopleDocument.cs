using System;
using System.Collections.Generic;

namespace FlmAutoRent.Data.Entities
{
    public partial class PeopleDocument
    {
        public string Id { get; set; }
        public string Path { get; set; }

        public PeopleDocumentType DocumentType { get; set; }
        public People People { get; set; }

    }
}