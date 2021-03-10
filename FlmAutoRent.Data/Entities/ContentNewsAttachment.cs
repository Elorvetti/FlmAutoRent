using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FlmAutoRent.Data.Entities
{
    public class ContentNewsAttachment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }        
        public ContentNews ContentNews { get; set; }
        public ContentAttachment ContentAttachment { get; set; }
    }
}