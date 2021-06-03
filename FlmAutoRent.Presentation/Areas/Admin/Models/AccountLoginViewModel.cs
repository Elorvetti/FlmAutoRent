using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace FlmAutoRent.Presentation.Areas.Admin.Models
{
    public class AccountLoginViewModel : BaseModel
    {

        [Display(Name="User ID")]    
        [Required(ErrorMessage="Il campo è obbligatorio")]
        public string UserID { get; set; }

        public string UserIdCrypt => ConvertToEncrypt(UserID);

        [Display(Name="Password")]    
        [Required(ErrorMessage="Il campo è obbligatorio")]
        [MinLength(8, ErrorMessage="Il campo deve contenere almeno 8 caratteri")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string PasswordCrypt => ConvertToEncrypt(Password);

    }
}