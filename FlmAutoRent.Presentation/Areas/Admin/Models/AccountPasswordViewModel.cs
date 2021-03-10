using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FlmAutoRent.Presentation.Areas.Admin.Models
{
    public class AccountPasswordViewModel
    {
        public Guid AccountGuid { get; set; }

        [Display(Name="Password")]    
        [Required(ErrorMessage="Il campo è obbligatorio")]
        [MinLength(8, ErrorMessage="Il campo deve contenere almeno 8 caratteri")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z])(?=.*[!-_?|£$%&,.;]).{8,}$", ErrorMessage="La Password deve contere almeno un carattere speciale, una lettera maiuscola, un numero")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name="Conferma password")]    
        [Required(ErrorMessage="Il campo è obbligatorio")]
        [Compare("Password", ErrorMessage = "La password non corrisponde")]
        [DataType(DataType.Password)]
        public string ConfirmPassword{ get; set; }


    }
}