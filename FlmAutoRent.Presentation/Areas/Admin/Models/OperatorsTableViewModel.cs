using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace FlmAutoRent.Presentation.Areas.Admin.Models
{
    public class OperatorsTableViewModel : TableViewModel
    {
        public OperatorsTableViewModel(){
            OperatatorListViewModel = new List<OperatorsViewModel>();
        }

        public List<OperatorsViewModel> OperatatorListViewModel { get; set; } = new List<OperatorsViewModel>();
    }

    public class OperatorsViewModel {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNr { get; set; }
        public string Group { get; set; }
        public int Enabled { get; set; }
    }
}