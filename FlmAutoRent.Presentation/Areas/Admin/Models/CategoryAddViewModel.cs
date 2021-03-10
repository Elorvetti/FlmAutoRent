using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using FlmAutoRent.Data.Entities; 

namespace FlmAutoRent.Presentation.Areas.Admin.Models
{
    public class CategoryAddViewModel
    {
        public CategoryAddViewModel(){
            ImagesList = new List<FileViewModel>();
        }
        public int Id { get; set; }
        public int IdFather { get; set; }

        [Required(ErrorMessage="Il campo è obbligatorio")]
        [MaxLength(150, ErrorMessage="Il Campo può contenere un massimo di 150 caratteri")]
        [RegularExpression(@"^[a-zA-Z ,.-_:;&]+$", ErrorMessage="Formato non valido")]
        [Display(Name="Titolo")]
        public string Name { get; set; }

        [Display(Name="Descrizione")]
        public string Description { get; set; }

        public bool DisplayRef { get; set; }
        
        [Display(Name="Pubblico")]
        public bool Display { get; set; }

        [Display(Name="Ordinamento")]
        public int Priority { get; set; }

        public int ImageListSelected { get; set; }
        public IList<FileViewModel> ImagesList { get; set; } = new List<FileViewModel>();
    }
}