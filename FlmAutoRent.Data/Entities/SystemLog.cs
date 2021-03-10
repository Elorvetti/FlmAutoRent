using System;

namespace FlmAutoRent.Data.Entities
{
    public partial class SystemLog
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public string EventType { get; set; }
        public string Value { get; set; }

        public ProfilingOperator Operators { get; set; }
    }
}