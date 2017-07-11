using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UnionManager.Models.EntityModels
{
    internal class TradeMetaData
    {
        [ScaffoldColumn(false)]
        [Bindable(false)]
        public long Id { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "نام صنف را وارد کنید")]
        [DisplayName("نام صنف")]
        [Display(Name = "نام صنف")]
        [StringLength(50, ErrorMessage = "این فیلد باید حداکثر 50 کاراکتر باشد")]
        public string Name { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "تلفن ثابت را وارد کنید")]
        [DisplayName("تلفن ثابت")]
        [Display(Name = "تلفن ثابت")]
        [StringLength(11, ErrorMessage = "این فیلد باید حداکثر 11 کاراکتر باشد")]
        public string Tel { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "آدرس را وارد کنید")]
        [DisplayName("آدرس")]
        [Display(Name = "آدرس")]
        public string Address { get; set; }


        [DisplayName("وظعیت")]
        [Display(Name = "وظعیت")]
        [StringLength(50, ErrorMessage = "این فیلد باید حداکثر 50 کاراکتر باشد")]
        public string Status { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "گروه صنف را انتخاب کنید")]
        [DisplayName("گروه صنف")]
        [Display(Name = "گروه صنف")]
        public long TradeGroupId { get; set; }
    }
}

namespace UnionManager.Models.DomainModels
{
    [MetadataType(typeof(UnionManager.Models.EntityModels.TradeMetaData))]
    public partial class Trade
    {
    }
}