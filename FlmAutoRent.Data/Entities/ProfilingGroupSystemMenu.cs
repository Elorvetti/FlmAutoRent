using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlmAutoRent.Data.Entities
{
    public class ProfilingGroupSystemMenu
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public ProfilingGroup Groups { get; set; }
        public ProfilingSystemMenu SystemMenus { get; set; }
    }
}