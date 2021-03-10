namespace FlmAutoRent.Data.Entities
{
    public class ProfilingOperatorEmail
    {
        public int Id { get; set; }
        public ProfilingOperator Operators { get; set; }
        public SystemEmail SystemEmails { get; set; }
    }
}