using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using FlmAutoRent.Data.Entities; 

namespace FlmAutoRent.Presentation.Areas.Admin.Models
{
    public class EmailAddViewModel : BaseModel
    {

        public EmailAddViewModel(){
            SystemOperatorEmail = new List<OperatorEmail>();
            EmailSSL = new List<EmailSSL>{
                new EmailSSL{ Id = 1, CryptType = "Nessuna" },
                new EmailSSL{ Id = 2, CryptType = "SSL" },
                new EmailSSL{ Id = 3, CryptType = "TLS" }
            };
        }
        public int Id { get; set; }

        [Display(Name="Provider")]        
        public IEnumerable<SelectListItem> DefaultEmailConfig { get; set; }

        [Required(ErrorMessage="Il campo è obbligatorio")]
        [MaxLength(50)]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage="Formato non valido")]
        [Display(Name="Nome")]
        public string Name { get; set; }

        [Required(ErrorMessage="Il campo è obbligatorio")]
        [MaxLength(250)]
        [EmailAddress(ErrorMessage = "Formato email non valido")]
        [Display(Name="Indirizzo Email")]        
        public string Email { get; set; }

        [Required(ErrorMessage="Il campo è obbligatorio")]
        [MaxLength(250)]
        [RegularExpression(@"^[a-zA-Z 0-9 .]+$", ErrorMessage="Formato non valido")]
        [Display(Name="Indirizzo POP3")]
        public string EmailPop { get; set; }

        [Required(ErrorMessage="Il campo è obbligatorio")]
        [Display(Name="Porta POP3")]
        public int EmailPortPop { get; set; }

        [Required(ErrorMessage="Il campo è obbligatorio")]
        [MaxLength(250)]
        [RegularExpression(@"^[a-zA-Z 0-9 .]+$", ErrorMessage="Formato non valido")]
        [Display(Name="Indirizzo SMTP")]
        public string EmailSmtp { get; set; }

        [Required(ErrorMessage="Il campo è obbligatorio")]
        [Display(Name="Porta SMTP")]
        public int EmailPortSmtp { get; set; }

        [Required(ErrorMessage="Il campo è obbligatorio")]
        [MaxLength(250)]
        [EmailAddress(ErrorMessage = "Formato email non valido")]
        [Display(Name="Username")]        
        public string Username { get; set; }

        public string UsernameCrypt => ConvertToEncrypt(Username);

        [Required(ErrorMessage="Il campo è obbligatorio")]
        [Display(Name="Password")]    
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string PasswordCrypt => ConvertToEncrypt(Password);

        [Display(Name="Firma")] 
        public string Signature { get; set; }

        public int EmailSSLValue { get; set; }
        public List<EmailSSL> EmailSSL { get; private set; }
        
        public List<OperatorEmail> SystemOperatorEmail { get; set; }
    
    }

    public class OperatorEmail : ProfilingOperatorEmail 
    {
        public bool Active{ get; set; }
    }

    public class EmailSSL 
    {
        public int Id { get; set; }
        public string CryptType { get; set; }
    }
}