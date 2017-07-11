using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnionManager.ViewModels.Boss
{
    public class BossViewModel
    {
        public List<UnionManager.Models.DomainModels.Relation> Relations { get; set; }

        public UnionManager.Models.DomainModels.Trade Trade { get; set; }

        public UnionManager.Models.DomainModels.User User { get; set; }

        public UnionManager.Models.DomainModels.Vehicle Vehicle { get; set; }

        public List<UnionManager.Models.DomainModels.VehicleGroup> VehicleGroups { get; set; }

        public List<UnionManager.Models.DomainModels.Color> Colors { get; set; }
    }
}