using System;

namespace FlmAutoRent.Data.Entities
{
    public class ProfilingOperatorPasswordHistory
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public DateTime InsertData { get; set; }
        public ProfilingOperator Operators { get; set; }
    }
}