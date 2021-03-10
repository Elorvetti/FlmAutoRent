using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace FlmAutoRent.Presentation.Models
{
    public class VehicleViewModel
    {
        public VehicleViewModel(){
            vehiclesImages = new List<VehiclesImage>();
            ContactModal = new ContactModal();
        }

        public int Id { get; set; }
        public string BrandImage { get; set; }
        public string Brand { get; set; }
        public string ModelName { get; set; }
        public string Description { get; set; }
        public List<VehiclesImage> vehiclesImages { get; set; } = new List<VehiclesImage>();
        public ContactModal ContactModal { get; set; } = new ContactModal();
        
    }

    public class VehiclesImage
    {
        public int Id { get; set; }
        public string Path{ get; set; }
        public string Alt{ get; set; }
    }

    public class ContactModal 
    {
        [Required(ErrorMessage="Il campo è obbligatorio")]
        [MaxLength(100)]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage="Formato non valido")]
        [Display(Name="Nome")]
        public string Name { get; set; }

        [Required(ErrorMessage="Il campo è obbligatorio")]
        [MaxLength(100)]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage="Formato non valido")]
        [Display(Name="Cognome")]
        public string Lastname { get; set; }
        
        [Required(ErrorMessage="Il campo è obbligatorio")]
        [MaxLength(250)]
        [EmailAddress(ErrorMessage = "Formato email non valido")]
        [Display(Name="Indirizzo Email")]    
        public string Email { get; set; }
        
        [Display(Name="Messaggio")]    
        public string Message { get; set; }

        [Required(ErrorMessage="Il campo è obbligatorio")]
        [Display(Name="Data Inizio Noleggio")]    
        public DateTime StartRent { get; set; }

        [Required(ErrorMessage="Il campo è obbligatorio")]
        [Display(Name="Data Fine Noleggio")] 
        public DateTime EndRent { get; set; }
    }
}