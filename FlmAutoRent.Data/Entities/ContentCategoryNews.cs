using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlmAutoRent.Data.Entities
{
    public partial class ContentCategoryNews
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public ContentCategory ContentCategories { get; set; }
        public ContentNews News { get; set; }
        public Vehicle Vehicle { get; set; }

    }
}