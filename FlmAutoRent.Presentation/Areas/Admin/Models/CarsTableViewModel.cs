using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using FlmAutoRent.Data.Entities; 

namespace FlmAutoRent.Presentation.Areas.Admin.Models
{
    public class CarsTableViewModel : TableViewModel
    {
        public CarsTableViewModel(){
            CarsListViewModel = new List<CarsViewModel>();
        }

        public List<CarsViewModel> CarsListViewModel { get; set; } 
    }

    public class CarsViewModel {
        public int Id { get; set; }
        public string PathLogo { get; set; }
        public string Category { get; set; }
        public string Model { get; set; }
        public int TotalRequest{ get; set; }
    }
}