using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnionManager.ViewModels.Vehicle
{
    public class VehicleDetailViewModel
    {
        public UnionManager.Models.DomainModels.Vehicle Vehicle { get; set; }

        public string UserVehicles { get; set; }

        public string VehicleTrades { get; set; }
    }
}