namespace FlmAutoRent.Data.Entities
{
    public class ContentCategoryImage
    {
        public int Id { get; set; }
        public ContentCategory Categories { get; set; }
        public ContentImage Images { get; set; }
    }
}