using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UnionManager.Models.ExcelModels
{
    public partial class VehicleExcelModel
    {
        public string NumberPlates { get; set; }

        public string Model { get; set; }

        public string ColorName { get; set; }

        public string VINName { get; set; }

        public string IsHybrid { get; set; }

        public string Status { get; set; }

        public string GroupName { get; set; }

        public string UsersName { get; set; }

        public string TradesName { get; set; }
    }
}