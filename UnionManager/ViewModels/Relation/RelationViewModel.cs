using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnionManager.ViewModels.Relation
{
    public class RelationViewModel
    {
        public List<UnionManager.Models.DomainModels.Relation> Relations { get; set; }

        public UnionManager.Models.DomainModels.Trade Trade { get; set; }

        public List<UnionManager.Models.DomainModels.Trade> Trades { get; set; }

        public List<UnionManager.Models.DomainModels.TradeGroup> TradeGroups { get; set; }

        public UnionManager.Models.DomainModels.User User { get; set; }

        public List<UnionManager.Models.DomainModels.User> Users { get; set; }

        public List<UnionManager.Models.DomainModels.Role> Roles { get; set; }

        public UnionManager.Models.DomainModels.Vehicle Vehicle { get; set; }

        public List<UnionManager.Models.DomainModels.Vehicle> Vehicles { get; set; }

        public List<UnionManager.Models.DomainModels.VehicleGroup> VehicleGroups { get; set; }

        public List<UnionManager.Models.DomainModels.Color> Colors { get; set; }
    }
}