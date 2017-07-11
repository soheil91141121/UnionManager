using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnionManager.ViewModels.Trade
{
    public class AddTradeViewModel
    {
        public List<UnionManager.Models.DomainModels.TradeGroup> TradeGroups { get; set; }

        public UnionManager.Models.DomainModels.Trade Trade { get; set; }
    }
}