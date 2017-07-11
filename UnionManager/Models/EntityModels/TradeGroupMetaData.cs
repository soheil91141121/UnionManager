using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UnionManager.Models.EntityModels
{
    internal class TradeGroupMetaData
    {
        [ScaffoldColumn(false)]
        [Bindable(false)]
        public long Id { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "گروه صنف را وارد کنید")]
        [DisplayName("گروه صنف")]
        [Display(Name = "گروه صنف")]
        [StringLength(50, ErrorMessage = "این فیلد باید حداکثر 50 کاراکتر باشد")]
        public string Name { get; set; }


        [DisplayName("وظعیت")]
        [Display(Name = "وظعیت")]
        [StringLength(50, ErrorMessage = "این فیلد باید حداکثر 50 کاراکتر باشد")]
        public string Status { get; set; }
    }
}

namespace UnionManager.Models.DomainModels
{
    [MetadataType(typeof(EntityModels.TradeGroupMetaData))]
    public partial class TradeGroup
    {
    }
}