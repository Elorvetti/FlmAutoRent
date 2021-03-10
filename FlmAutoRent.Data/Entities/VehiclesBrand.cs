using System;
using System.Collections.Generic;

namespace FlmAutoRent.Data.Entities
{
    public class VehiclesBrand
    {
        public VehiclesBrand(){
            VehiclesMappings = new HashSet<VehiclesMapping>();
        }
        public int Id { get; set; }
        public string BrandName { get; set; }
        public string Description { get; set; }
        public string BrandImagePath { get; set; }
        public string PermaLink { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public int SeoIndexRef { get; set;}
        public DateTime OperatorData { get; set; }
        public int IDOperator { get; set; }
        
        public ICollection<VehiclesMapping> VehiclesMappings { get; set; }
        public SeoIndex SeoIndex { get; set; }
    }
}

