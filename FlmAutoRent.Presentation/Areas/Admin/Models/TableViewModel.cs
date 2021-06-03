using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;


namespace FlmAutoRent.Presentation.Areas.Admin.Models
{
    public class TableViewModel : PaginationViewModel
    {
        public TableViewModel(){
            HowManyFieldList = new List<HowManyFields>{ 
                new HowManyFields { Value = 10, DisplayText="Visualizza 10 risultati per pagina" }, 
                new HowManyFields { Value = 100, DisplayText="Visualizza 100 risultati per pagina" }, 
                new HowManyFields { Value = 1000, DisplayText="Visualizza 1000 risultati per pagina" }, 
                new HowManyFields { Value = 100000, DisplayText="Visualizza tutti i risultati per pagina" } 
            };
        }

        [MinLength(3)]
        [MaxLength(100)]
        [RegularExpression(@"^[a-zA-Z]+$")]
        public string Find { get; set; }
        public int HowManyField { get; set; }
        public List<HowManyFields> HowManyFieldList{ get; private set; }


        public class HowManyFields{
            public int Value { get; set; }
            public string DisplayText { get; set; }
        }
    }
}