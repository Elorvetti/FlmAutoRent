using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using FlmAutoRent.Data.Entities; 

namespace FlmAutoRent.Presentation.Areas.Admin.Models
{
    public class HomepageViewModel
    {
        public HomepageViewModel(){
            ContentCategories = new List<ContentCategory>();
            Vehicles = new List<VehicleCard>();
            HeaderImagesList = new List<FileViewModel>();
            PresentationImagesList = new List<FileViewModel>();
        }

        public List<ContentCategory> ContentCategories { get; set; } = new List<ContentCategory>();
        public List<VehicleCard> Vehicles { get; set; } = new List<VehicleCard>();
        public string ContentImageHeader { get; set; }
        public string ContentImagePresentation { get; set; }
        public string WelcomeMessage { get; set; }
        public string Presentation { get; set; }

        public int HeaderImageListSelected { get; set; }
        public IList<FileViewModel> HeaderImagesList { get; set; } = new List<FileViewModel>();

        public int PresentationImageListSelected { get; set; }
        public IList<FileViewModel> PresentationImagesList { get; set; } = new List<FileViewModel>();
    }

    public class VehicleCard
    {
        public int Id { get; set; }
        public string srcImg { get; set; }
        public string altImg { get; set; }
        public string vehiclebrand { get; set; }
        public string vehicleModel { get; set; }
        public string vehicleDescription { get; set; }
        public string permaLink { get; set; }
        
    }
}