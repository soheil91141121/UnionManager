using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UnionManager.Models.ExcelModels
{
    public partial class UserExcelModel
    {
        public string Name { get; set; }

        public string Family { get; set; }

        public string FatherName { get; set; }

        public string Gender { get; set; }

        public string NationalCode { get; set; }

        public string IdNo { get; set; }

        public string BirthDate { get; set; }

        public string Mobile { get; set; }

        public string Tel { get; set; }

        public string Status { get; set; }

        public string RoleName { get; set; }

        public string Username { get; set; }

        public string IsInsuranced { get; set; }

        public string TradesName { get; set; }

        public string VehiclesName { get; set; }
    }
}