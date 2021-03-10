using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using FlmAutoRent.Data.Entities; 

namespace FlmAutoRent.Presentation.Areas.Admin.Models
{
    public class NewsTableViewModel : TableViewModel
    {
        public NewsTableViewModel(){
            NewsListViewModel = new List<NewsViewModel>();
        }

        public List<NewsViewModel> NewsListViewModel { get; set; } 

    }

    public class NewsViewModel {
        public int Id { get; set; }
        public DateTime PublicDate { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Public { get; set; }
    }
    
}