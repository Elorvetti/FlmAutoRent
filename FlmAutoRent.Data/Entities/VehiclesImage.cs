using System;
using System.Collections.Generic;

namespace FlmAutoRent.Data.Entities
{
    public class VehiclesImage
    {
        public VehiclesImage(){
            VehiclesImages = new HashSet<VehiclesImagesMapping>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public int Priority { get; set; }
        public ICollection<VehiclesImagesMapping> VehiclesImages { get; set; }
    }
}