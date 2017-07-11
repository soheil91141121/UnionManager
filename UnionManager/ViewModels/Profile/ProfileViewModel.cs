using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnionManager.ViewModels.Profile
{
    public class ProfileViewModel
    {
        public UnionManager.Models.DomainModels.User User { get; set; }

        public UnionManager.Models.DomainModels.User RealUser { get; set; }

        public string Trades { get; set; }

        public string Vehicles { get; set; }
    }
}