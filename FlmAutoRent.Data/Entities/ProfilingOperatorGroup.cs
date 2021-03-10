namespace FlmAutoRent.Data.Entities
{
    public class ProfilingOperatorGroup
    {
        public int Id { get; set; }
        public ProfilingOperator Operators { get; set; }
        public ProfilingGroup Groups { get; set; }
    }
}