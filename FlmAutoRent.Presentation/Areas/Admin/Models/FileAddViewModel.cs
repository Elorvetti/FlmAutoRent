using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace FlmAutoRent.Presentation.Areas.Admin.Models
{
    public class FileAddViewModel
    {
        public int Id { get; set; }

        
        [Display(Name="Clicca qui per caricare un'immagine")]
        public IFormFile Upload { get; set; }

        [Required(ErrorMessage="Il campo è obbligatorio")]
        [Display(Name="Titolo")]
        [MaxLength(250, ErrorMessage="Il campo consente l'inserimento di massimo 250 caratteri")]
        public string Title { get; set; }

        [Display(Name="Descrizione")]
        public string Description { get; set; }

        [Display(Name="Altezza")]
        public int FileHeight { get; set; }

        [Display(Name="Larghezza")]
        public int FileWidth { get; set; }

        
        [Display(Name="Dimensioni")]
        public int FileSize { get; set; }

        public string FilePath { get; set; }
    }
}