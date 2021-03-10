using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using FlmAutoRent.Data.Entities; 

namespace FlmAutoRent.Presentation.Areas.Admin.Models
{
    public class BrandsTableViewModel : TableViewModel
    {
        public BrandsTableViewModel(){
            BrandsListViewModel = new List<BrandsViewModel>();
        }

        public List<BrandsViewModel> BrandsListViewModel { get; set; } 
    }

    public class BrandsViewModel {
        public int Id { get; set; }
        public string PathLogo { get; set; }
        public string Name { get; set; }
        public int Nusing{ get; set; }
    }
}