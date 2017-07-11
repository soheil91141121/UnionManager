using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UnionManager.Models.EntityModels
{
    internal class RoleMetaData
    {
        [ScaffoldColumn(false)]
        [Bindable(false)]
        public long Id { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "نقش کاربر را وارد کنید")]
        [DisplayName("نقش کاربر")]
        [Display(Name = "نقش کاربر")]
        [StringLength(50, ErrorMessage = "این فیلد باید حداکثر 50 کاراکتر باشد")]
        public string Name { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "نقش کاربر را وارد کنید")]
        [DisplayName("نقش کاربر")]
        [Display(Name = "نقش کاربر")]
        [StringLength(50, ErrorMessage = "این فیلد باید حداکثر 50 کاراکتر باشد")]
        public string RoleName { get; set; }


        [DisplayName("وظعیت")]
        [Display(Name = "وظعیت")]
        [StringLength(50, ErrorMessage = "این فیلد باید حداکثر 50 کاراکتر باشد")]
        public string Status { get; set; }
    }
}

namespace UnionManager.Models.DomainModels
{
    [MetadataType(typeof(UnionManager.Models.EntityModels.RoleMetaData))]
    public partial class Role
    {
    }
}