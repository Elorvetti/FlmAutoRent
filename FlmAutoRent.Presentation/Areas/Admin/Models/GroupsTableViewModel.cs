using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using FlmAutoRent.Data.Entities; 

namespace FlmAutoRent.Presentation.Areas.Admin.Models
{
    public class GroupsTableViewModel : TableViewModel
    {
        public GroupsTableViewModel(){
            GroupsListViewModel = new List<GroupsViewModel>();
        }

        public List<GroupsViewModel> GroupsListViewModel { get; set; } 

    }

    public class GroupsViewModel {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int OperatorsInThisGroup{ get; set; }
    }
    
}