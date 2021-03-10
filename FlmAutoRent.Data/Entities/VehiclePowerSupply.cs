using System;
using System.Collections.Generic;

namespace FlmAutoRent.Data.Entities
{
    public class VehiclePowerSupply
    {
        public VehiclePowerSupply(){
            VehiclesMappings = new HashSet<VehiclesMapping>();
        }
        public int Id { get; set; }
        public string PowerSupply { get; set; }
        public ICollection<VehiclesMapping> VehiclesMappings { get; set; }

    }
}