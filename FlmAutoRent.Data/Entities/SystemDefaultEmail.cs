namespace FlmAutoRent.Data.Entities
{
    public class SystemDefaultEmail
    {
        public int Id { get; set; }
        public string EmailProvider { get; set; }
        public string EmailSmtp { get; set; }
        public int EmailPortSmtp { get; set; }
        public string EmailPop { get; set; }
        public int EmailPortPop { get; set;}
        public int EmailSendUsing { get; set; }
        public int EmailSSL { get; set; }
        public int EmailAutenticate { get; set; }
    }
}