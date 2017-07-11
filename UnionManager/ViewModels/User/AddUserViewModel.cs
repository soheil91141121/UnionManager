using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnionManager.ViewModels.User
{
    public class AddUserViewModel
    {
        public List<UnionManager.Models.DomainModels.Role> Roles { get; set; }

        public UnionManager.Models.DomainModels.User User { get; set; }
    }
}