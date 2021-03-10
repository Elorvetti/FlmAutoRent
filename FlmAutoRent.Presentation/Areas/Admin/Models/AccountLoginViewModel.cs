using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FlmAutoRent.Presentation.Areas.Admin.Models
{
    public class AccountLoginViewModel
    {

        [Display(Name="User ID")]    
        [Required(ErrorMessage="Il campo è obbligatorio")]
        public string UserID { get; set; }

        [Display(Name="Password")]    
        [Required(ErrorMessage="Il campo è obbligatorio")]
        [MinLength(8, ErrorMessage="Il campo deve contenere almeno 8 caratteri")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}