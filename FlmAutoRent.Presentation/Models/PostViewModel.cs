using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace FlmAutoRent.Presentation.Models
{
    public class PostViewModel
    {
        public PostViewModel(){
            Cards = new List<VehicleCard>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DisplayText { get; set; }

        public List<VehicleCard> Cards { get; set; } = new List<VehicleCard>();
    }

}