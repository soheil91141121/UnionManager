using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnionManager.ViewModels.User
{
    public class UserViewModel
    {
        public List<UnionManager.Models.DomainModels.User> Users { get; set; }

        public UnionManager.Models.DomainModels.User User { get; set; }

        public List<UnionManager.Models.DomainModels.Role> Roles { get; set; }

        public UnionManager.Models.DomainModels.Role Role { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPageCount { get; set; }

        public int TotalItemCount { get; set; }

        public long TotalModelCount { get; set; }
    }
}