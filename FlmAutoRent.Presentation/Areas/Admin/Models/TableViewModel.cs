using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;


namespace FlmAutoRent.Presentation.Areas.Admin.Models
{
    public class TableViewModel
    {
        public TableViewModel(){
            HowManyFieldList = new List<HowManyFields>();
        }

        [MinLength(3)]
        [MaxLength(100)]
        [RegularExpression(@"^[a-zA-Z]+$")]
        public string Find { get; set; }
        public int HowManyField { get; set; }
        public List<HowManyFields> HowManyFieldList{ get; set; }


        public class HowManyFields{
            public int Value { get; set; }
            public string DisplayText { get; set; }
        }
    }
}