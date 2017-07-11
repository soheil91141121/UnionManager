using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnionManager.ViewModels.GroupAndColor
{
    public class GroupViewModel
    {
        public UnionManager.Models.DomainModels.TradeGroup TradeGroup { get; set; }

        public List<UnionManager.Models.DomainModels.TradeGroup> TradeGroups { get; set; }

        public UnionManager.Models.DomainModels.VehicleGroup VehicleGroup { get; set; }

        public List<UnionManager.Models.DomainModels.VehicleGroup> VehicleGroups { get; set; }

        public UnionManager.Models.DomainModels.Color Color { get; set; }

        public List<UnionManager.Models.DomainModels.Color> Colors { get; set; }
    }
}