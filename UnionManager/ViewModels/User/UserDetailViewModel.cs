using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnionManager.ViewModels.User
{
    public class UserDetailViewModel
    {
        public UnionManager.Models.DomainModels.User User { get; set; }

        public string UserVehicles { get; set; }

        public string UserTrades { get; set; }
    }
}