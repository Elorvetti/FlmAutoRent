using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace FlmAutoRent.Presentation.Models
{
    public class CategoryViewModel
    {
        public CategoryViewModel(){
            Cards = new List<CardViewModel>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public List<CardViewModel> Cards { get; set; } = new List<CardViewModel>();
    }

    public class CardViewModel 
    {
        public string pathImg { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Permalink { get; set; }
    }
}