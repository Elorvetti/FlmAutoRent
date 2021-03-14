using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using FlmAutoRent.Data.Entities; 

namespace FlmAutoRent.Presentation.Areas.Admin.Models
{
    public class OperatorAddViewModel
    {

        public OperatorAddViewModel(){
            EmailsAccount = new List<EmailAccount>();
        }
        public int Id { get; set; }

        [Required(ErrorMessage="Il campo è obbligatorio")]
        [MaxLength(50, ErrorMessage="La lunghezza del campo non deve superare i 50 caratteri")]
        [RegularExpression(@"^[a-zA-Z ,.-_:;]+$", ErrorMessage="Formato non valido")]
        [Display(Name="User ID")]
        public string UserId { get; set; }

        [Required(ErrorMessage="Il campo è obbligatorio")]
        [MaxLength(50)]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage="Formato non valido")]
        [Display(Name="Nome")]
        public string Name { get; set; }

        [Required(ErrorMessage="Il campo è obbligatorio")]
        [MaxLength(50)]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage="Formato non valido")]
        [Display(Name="Cognome")]
        public string LastName { get; set; }

        [Required(ErrorMessage="Il campo è obbligatorio")]
        [MaxLength(250)]
        [EmailAddress(ErrorMessage = "Formato email non valido")]
        [Display(Name="Indirizzo Email")]        
        public string EmailAddress { get; set; }

        [MaxLength(18)]
        [Display(Name="Telefono")]        
        public string PhoneNr { get; set; }

        public int EnabledId { get; set; }
        
        [Display(Name="Stato")]        
        public IEnumerable<SelectListItem> Enabled { get; set; }

        public int GroupId { get; set; }
        
        [Display(Name="Gruppo Appartenenza")]        
        public IEnumerable<SelectListItem> GroupList { get; set; }

        [Display(Name="Password")]    
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public int PasswordDeadLineValue { get; set; }
        
        [Display(Name="Scadenza Password")]    
        public IEnumerable<SelectListItem> PasswordDeadline { get; set; }
            
        public List<int> EmailAccountId { get; set; }  = new List<int>();
        public List<EmailAccount> EmailsAccount { get; set; } = new List<EmailAccount>();
    
    }
    public class EmailAccount :  SystemEmail
    {
        public bool Active{ get; set; }
    }

    public class ErrorViewModel 
    {
        public string Error { get; set; }
    }

}