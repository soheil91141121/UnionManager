using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnionManager.ViewModels.Vehicle
{
    public class AddVehicleViewModel
    {
        public List<UnionManager.Models.DomainModels.VehicleGroup> VehicleGroups { get; set; }

        public List<UnionManager.Models.DomainModels.Color> Colors { get; set; }

        public UnionManager.Models.DomainModels.Vehicle Vehicle { get; set; }
    }
}