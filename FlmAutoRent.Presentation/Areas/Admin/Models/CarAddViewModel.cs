using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using FlmAutoRent.Data.Entities; 

namespace FlmAutoRent.Presentation.Areas.Admin.Models
{
    public class CarAddViewModel
    {
        public int Id { get; set; }
        public CarFirstStep carFirstStep { get; set; } = new CarFirstStep();
        public CarSecondStep carSecondStep{ get; set; } = new CarSecondStep();
        public List<CarSecondStep> carSecondStepMenage { get; set; } = new List<CarSecondStep>();
        public ThirdStep thirdStep { get; set; } = new ThirdStep();
        public List<FourStep> carFourStep { get; set; } = new List<FourStep>();
        public bool disableSecondStep { get; set; }
        public bool disableThirdStep { get; set; }
        public bool disableFourStep { get; set; }
    }

    public class CarFirstStep{
        
        public int CategorySelected { get; set; }
        public List<ContentCategory> Categories { get; set; } = new List<ContentCategory>();

        public int BrandId { get; set; }
        
        [Display(Name="Brand")]        
        public IEnumerable<SelectListItem> BrandList { get; set; }

        public int PowerSupplyId { get; set; }
        
        [Display(Name="Alimentazione")]        
        public IEnumerable<SelectListItem> PowerSupplyList { get; set; }

        [MaxLength(255, ErrorMessage="IL campo non deve superare i 255 caratteri")]
        [Display(Name="Modello")]
        public string Model { get; set; }

        [MaxLength(255, ErrorMessage="IL campo non deve superare i 255 caratteri")]
        [Display(Name="Descrizione")]
        public string Description { get; set; }

        [Display(Name="CV")]
        public string CV { get; set; }

        [Display(Name="KW")]
        public string KW { get; set; }

        [Display(Name="Prenotabile")]
        public bool Bookable { get; set; }
        
        [Display(Name="Visualizza in Home Page")]
        public bool DisplayHp { get; set; }

        [Display(Name="Ordinamento")]
        public int Priority { get; set; }
    }

    public class CarSecondStep {
        public int ImageId { get; set; }
        
        [Display(Name="Clicca qui per caricare un'immagine")]
        public IFormFile Upload { get; set; }
        
        [Display(Name="Titolo")]
        [MaxLength(250, ErrorMessage="Il campo consente l'inserimento di massimo 250 caratteri")]
        public string Title { get; set; }
        
        [Display(Name="Descrizione")]
        public string Description { get; set; }

        [Display(Name="Ordinamento")]
        public int Priority { get; set; }
        public string FilePath { get; set; }
    }

    public class ThirdStep {
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

    public class FourStep{
        public int MessageId { get; set; }
        public string PeopleEmail { get; set; }
        public string PeopleName { get; set; }
        public string PeopleLastname { get; set; }
        public string PeopleMessage { get; set; }
    }
}