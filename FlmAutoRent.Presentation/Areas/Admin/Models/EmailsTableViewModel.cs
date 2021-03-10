using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using FlmAutoRent.Data.Entities; 

namespace FlmAutoRent.Presentation.Areas.Admin.Models
{
    public class EmailsTableViewModel : TableViewModel
    {
        public EmailsTableViewModel(){
            EmailsListViewModel = new List<EmailsViewModel>();
        }

        public List<EmailsViewModel> EmailsListViewModel { get; set; } 

    }

    public class EmailsViewModel {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Pop { get; set; }
        public string Smtp { get; set; }
        public int NUsing { get; set; }
    }
    
}