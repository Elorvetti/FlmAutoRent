using System;

namespace FlmAutoRent.Data.Entities
{
    public partial class SystemBlackList
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string IpAddress { get; set; }
        public string ReverseDNS { get; set; }
        public DateTime InsertData { get; set; }
    }
}