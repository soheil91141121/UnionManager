using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UnionManager.Models.EntityModels
{
    internal class VehicleMetaData
    {
        [ScaffoldColumn(false)]
        [Bindable(false)]
        public long Id { get; set; }


        [DisplayName("شماره پلاک")]
        [Display(Name = "شماره پلاک")]
        [StringLength(20, ErrorMessage = "این فیلد باید حداکثر 20 کاراکتر باشد")]
        public string NumberPlates { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "مدل را وارد کنید")]
        [DisplayName("مدل")]
        [Display(Name = "مدل")]
        [StringLength(50, ErrorMessage = "این فیلد باید حداکثر 50 کاراکتر باشد")]
        public string Model { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "رنگ را انتخاب کنید")]
        [DisplayName("رنگ")]
        [Display(Name = "رنگ")]
        public long ColorId { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "نام VIN را وارد کنید")]
        [DisplayName("نام VIN")]
        [Display(Name = "نام VIN")]
        [StringLength(50, ErrorMessage = "این فیلد باید حداکثر 50 کاراکتر باشد")]
        public string VINName { get; set; }


        [DisplayName("دوگانه سوز")]
        [Display(Name = "دوگانه سوز")]
        public bool IsHybrid { get; set; }


        [DisplayName("وظعیت")]
        [Display(Name = "وظعیت")]
        [StringLength(50, ErrorMessage = "این فیلد باید حداکثر 50 کاراکتر باشد")]
        public string Status { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "گروه وسیله نقلیه را انتخاب کنید")]
        [DisplayName("گروه وسیله نقلیه")]
        [Display(Name = "گروه وسیله نقلیه")]
        public long VehicleGroupId { get; set; }
    }
}

namespace UnionManager.Models.DomainModels
{
    [MetadataType(typeof(EntityModels.VehicleMetaData))]
    public partial class Vehicle
    {
    }
}