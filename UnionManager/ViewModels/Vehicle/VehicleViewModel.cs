using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnionManager.ViewModels.Vehicle
{
    public class VehicleViewModel
    {
        public List<UnionManager.Models.DomainModels.Vehicle> Vehicles { get; set; }

        public UnionManager.Models.DomainModels.Vehicle Vehicle { get; set; }

        public List<UnionManager.Models.DomainModels.VehicleGroup> VehicleGroups { get; set; }

        public UnionManager.Models.DomainModels.VehicleGroup VehicleGroup { get; set; }

        public List<UnionManager.Models.DomainModels.Color> Colors { get; set; }

        public UnionManager.Models.DomainModels.Color Color { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPageCount { get; set; }

        public int TotalItemCount { get; set; }

        public long TotalModelCount { get; set; }
    }
}