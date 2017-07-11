using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UnionManager.Models.EntityModels
{
    internal class UserMetaData
    {
        [ScaffoldColumn(false)]
        [Bindable(false)]
        public long Id { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "نام را وارد کنید")]
        [DisplayName("نام")]
        [Display(Name = "نام")]
        [StringLength(50, ErrorMessage = "این فیلد باید حداکثر 50 کاراکتر باشد")]
        public string Name { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "نام خانوادگی را وارد کنید")]
        [DisplayName("نام خانوادگی")]
        [Display(Name = "نام خانوادگی")]
        [StringLength(50, ErrorMessage = "این فیلد باید حداکثر 50 کاراکتر باشد")]
        public string Family { get; set; }


        [DisplayName("نام پدر")]
        [Display(Name = "نام پدر")]
        [StringLength(50, ErrorMessage = "این فیلد باید حداکثر 50 کاراکتر باشد")]
        public string FatherName { get; set; }


        [DisplayName("جنسیت")]
        [Display(Name = "جنسیت")]
        public bool Gender { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "کد ملی را وارد کنید")]
        [DisplayName("کد ملی")]
        [Display(Name = "کد ملی")]
        [StringLength(10, ErrorMessage = "این فیلد باید حداکثر 10 کاراکتر باشد")]
        public string NationalCode { get; set; }


        [DisplayName("شماره شناسنامه")]
        [Display(Name = "شماره شناسنامه")]
        [StringLength(10, ErrorMessage = "این فیلد باید حداکثر 10 کاراکتر باشد")]
        public string IdNo { get; set; }


        [DisplayName("تاریخ تولد")]
        [Display(Name = "تاریخ تولد")]
        public Nullable<System.DateTime> BirthDate { get; set; }


        [DisplayName("شماره همراه")]
        [Display(Name = "شماره همراه")]
        [StringLength(11, ErrorMessage = "این فیلد باید حداکثر 11 کاراکتر باشد")]
        public string Mobile { get; set; }


        [DisplayName("تلفن ثابت")]
        [Display(Name = "تلفن ثابت")]
        [StringLength(11, ErrorMessage = "این فیلد باید حداکثر 11 کاراکتر باشد")]
        public string Tel { get; set; }


        [DisplayName("تصویر")]
        [Display(Name = "تصویر")]
        public string Image { get; set; }


        [DisplayName("وظعیت")]
        [Display(Name = "وظعیت")]
        [StringLength(50, ErrorMessage = "این فیلد باید حداکثر 50 کاراکتر باشد")]
        public string Status { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "نقش کاربر را انتخاب کنید")]
        [DisplayName("نقش کاربر")]
        [Display(Name = "نقش کاربر")]
        public long RoleId { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "نام کاربری را وارد کنید")]
        [DisplayName("نام کاربری")]
        [Display(Name = "نام کاربری")]
        [StringLength(50, ErrorMessage = "این فیلد باید حداکثر 50 کاراکتر باشد")]
        public string Username { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "کلمه عبور را وارد کنید")]
        [DisplayName("کلمه عبور")]
        [Display(Name = "کلمه عبور")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("بیمه شده")]
        [Display(Name = "بیمه شده")]
        public bool IsInsuranced { get; set; }
    }
}

namespace UnionManager.Models.DomainModels
{
    [MetadataType(typeof(EntityModels.UserMetaData))]
    public partial class User
    {
        [Required(ErrorMessage = "کلمه عبور را تکرار کنید", AllowEmptyStrings = false)]
        [DisplayName("تکرار کلمه عبور")]
        [Display(Name = "تکرار کلمه عبور")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "تکرار رمز عبور یکسان نیست")]
        public string ConfirmPassword { get; set; }
    }
}