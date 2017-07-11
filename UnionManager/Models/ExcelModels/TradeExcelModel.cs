using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UnionManager.Models.ExcelModels
{
    public partial class TradeExcelModel
    {
        public string Name { get; set; }

        public string GroupName { get; set; }

        public string Tel { get; set; }

        public string Address { get; set; }

        public string Status { get; set; }

        public string UsersName { get; set; }

        public string VehiclesName { get; set; }
    }
}