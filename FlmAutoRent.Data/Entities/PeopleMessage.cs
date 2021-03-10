using System;
using System.Collections.Generic;

namespace FlmAutoRent.Data.Entities
{
    public class PeopleMessage
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime DateSend{ get; set; }

        public Vehicle Vehicle { get; set; }
        public People People { get; set; }

    }
}