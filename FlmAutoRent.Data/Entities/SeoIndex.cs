using System;
using System.Collections.Generic;

namespace FlmAutoRent.Data.Entities
{
    public class SeoIndex
    {
        public int Id { get; set; }        
        public string Description { get; set; }
        public ContentNews ContentNews { get; set; }
        public VehiclesBrand VehiclesBrand { get; set; }
        public Vehicle Vehicle { get; set; }
    }
}