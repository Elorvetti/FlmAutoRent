using System;
using System.Collections.Generic;

namespace FlmAutoRent.Data.Entities
{
    public class VehiclesMapping
    {
        public int Id { get; set; }
        public VehiclesBrand Brands{ get; set; }
        public VehiclesTransmission Transmission{ get; set; }
        public VehiclePowerSupply Supplies{ get; set; }
        public Vehicle Vehicles{ get; set; }
    }
}