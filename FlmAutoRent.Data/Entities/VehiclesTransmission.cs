using System;
using System.Collections.Generic;

namespace FlmAutoRent.Data.Entities
{
    public class VehiclesTransmission
    {
        public VehiclesTransmission(){
            VehiclesMappings = new HashSet<VehiclesMapping>();
        }
        
        public int Id { get; set; }
        public string Trasmission { get; set; }
        public ICollection<VehiclesMapping> VehiclesMappings { get; set; }

    }
}