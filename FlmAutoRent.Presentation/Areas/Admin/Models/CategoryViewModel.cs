using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using FlmAutoRent.Data.Entities; 

namespace FlmAutoRent.Presentation.Areas.Admin.Models
{
    public class CategoryViewModel
    {
        public CategoryViewModel(){
            Categories = new List<ContentCategory>();
        }
        public List<ContentCategory> Categories { get; set; } = new List<ContentCategory>();
    }
    
}