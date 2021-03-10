using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using FlmAutoRent.Data.Entities; 

namespace FlmAutoRent.Presentation.Areas.Admin.Models
{
    public class NewsAddViewModel
    {
        public NewsAddViewModel(){
            Categories = new List<ContentCategory>();
            ImagesList = new List<NewsFileViewModel>();
            VideoList = new List<NewsFileViewModel>();
            AttachmentList = new List<NewsFileViewModel>();
        }
        public int Id { get; set; }
        
        [Required(ErrorMessage="Il campo è obbligatorio")]
        [MaxLength(255, ErrorMessage="La lunghezza del campo non deve superare i 255 caratteri")]
        [Display(Name="Titolo")]
        public string Title{ get; set; }
        public Guid Guid{ get; set; }

        [MaxLength(255, ErrorMessage="La lunghezza del campo non deve superare i 255 caratteri")]
        [Display(Name="Sottotitolo")]
        public string SubTitle{ get; set; }
        
        [MaxLength(1000, ErrorMessage="La lunghezza del campo non deve superare i 1000 caratteri")]
        [Display(Name="Riassunto")]
        public string Summary{ get; set; }

        [Display(Name="Contenuto")]
        public string News{ get; set; }

        [Display(Name="PermaLink")]
        public string PermaLink{ get; set; }

        [Display(Name="Meta Description")]
        public string MetaDescription{ get; set; }

        [Display(Name="Meta Title")]
        public string MetaTitle{ get; set; }

        [Display(Name="Indicizzazione")]
        public int NoIndex{ get; set; }
        
        [Display(Name="Nel Footer")]
        public bool DisplayOnFooter{ get; set; }

        [Display(Name="Pubblica")]
        public bool DisplayNews{ get; set; }

        public DateTime OperatorData{ get; set; }
        public int IDOperator{ get; set; }
        public int CategorySelected { get; set; }
        public List<ContentCategory> Categories { get; set; } = new List<ContentCategory>();
        public IList<NewsFileViewModel> ImagesList { get; set; } = new List<NewsFileViewModel>();
        public IList<NewsFileViewModel> VideoList { get; set; } = new List<NewsFileViewModel>();
        public IList<NewsFileViewModel> AttachmentList { get; set; } = new List<NewsFileViewModel>();
        public int SeoIndexId { get; set; }

        [Display(Name="Indicizzazione")]        
        public IEnumerable<SelectListItem> SeoIndex { get; set; }
    }

    public class NewsFileViewModel : FileViewModel 
    {
        public bool Selected { get; set; }
    }
}