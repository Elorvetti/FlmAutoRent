using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FlmAutoRent.Data.Entities
{
    public class ContentNewsVideo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }        
        public ContentNews ContentNews { get; set; }
        public ContentVideo ContentVideo { get; set; }
    }
}