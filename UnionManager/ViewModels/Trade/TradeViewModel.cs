using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnionManager.ViewModels.Trade
{
    public class TradeViewModel
    {
        public List<UnionManager.Models.DomainModels.Trade> Trades { get; set; }

        public UnionManager.Models.DomainModels.Trade Trade { get; set; }

        public List<UnionManager.Models.DomainModels.TradeGroup> TradeGroups { get; set; }

        public UnionManager.Models.DomainModels.TradeGroup TradeGroup { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPageCount { get; set; }

        public int TotalItemCount { get; set; }

        public long TotalModelCount { get; set; }
    }
}