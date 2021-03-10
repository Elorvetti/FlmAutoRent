using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using FlmAutoRent.Data.Entities; 

namespace FlmAutoRent.Presentation.Areas.Admin.Models
{
    public class GroupAddViewModel
    {

        public GroupAddViewModel(){
            SystemMenuList = new List<SystemMenu>();
            OperatorsList = new List<OperatorList>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage="Il campo è obbligatorio")]
        [MaxLength(300)]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage="Formato non valido")]
        [Display(Name="Nome")]
        public string Name { get; set; }
        public List<SystemMenu> SystemMenuList { get; set; }
        public List<OperatorList> OperatorsList { get; set; }
    }

    public class SystemMenu : ProfilingSystemMenu 
    {
        public bool Active{ get; set; }
    }

    public class OperatorList : ProfilingOperator
    {
        public bool Active{ get; set; }
    }
}