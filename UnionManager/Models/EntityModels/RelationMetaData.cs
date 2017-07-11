using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UnionManager.Models.EntityModels
{
    internal class RelationMetaData
    {
        [ScaffoldColumn(false)]
        [Bindable(false)]
        public long Id { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "صنف را انتخاب کنید")]
        [DisplayName("صنف")]
        [Display(Name = "صنف")]
        public long TradeId { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "کاربر را انتخاب کنید")]
        [DisplayName("کاربر")]
        [Display(Name = "کاربر")]
        public long UserId { get; set; }


        [DisplayName("وسیله نقلیه")]
        [Display(Name = "وسیله نقلیه")]
        public Nullable<long> VehicleId { get; set; }


        [DisplayName("وظعیت")]
        [Display(Name = "وظعیت")]
        [StringLength(50, ErrorMessage = "این فیلد باید حداکثر 50 کاراکتر باشد")]
        public string Status { get; set; }
    }
}

namespace UnionManager.Models.DomainModels
{
    [MetadataType(typeof(EntityModels.RelationMetaData))]
    public partial class Relation
    {
    }
}