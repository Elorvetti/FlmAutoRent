namespace FlmAutoRent.Presentation.Areas.Admin.Models
{
    public class EmailSenderViewModel
    {
        public string EmailTemplate { get; set; }
        public string EmailObject { get; set; }
        public string EmailTo { get; set; }
        public string EmailBody { get; set; }
    }
}