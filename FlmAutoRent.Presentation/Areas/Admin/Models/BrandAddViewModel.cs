using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FlmAutoRent.Presentation.Areas.Admin.Models
{
    public class BrandAddViewModel
    {
        public int Id { get; set; }   

        [Display(Name="Clicca qui per caricare un'immagine")]
        public IFormFile Upload { get; set; }

        [Required(ErrorMessage="Il campo è obbligatorio")]
        [Display(Name="Titolo")]
        [MaxLength(250, ErrorMessage="Il campo consente l'inserimento di massimo 250 caratteri")]
        public string Nome { get; set; }

        [Display(Name="Descrizione")]
        public string Description { get; set; }

        public string LogoPath { get; set; }

        [Display(Name="PermaLink")]
        public string PermaLink{ get; set; }

        [Display(Name="Meta Description")]
        public string MetaDescription{ get; set; }

        [Display(Name="Meta Title")]
        public string MetaTitle{ get; set; }

        [Display(Name="Indicizzazione")]
        public int NoIndex{ get; set; }

        public int SeoIndexId { get; set; }
        [Display(Name="Indicizzazione")]        
        public IEnumerable<SelectListItem> SeoIndex { get; set; }        
    }
}