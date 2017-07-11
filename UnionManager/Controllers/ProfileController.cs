using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UnionManager.Models.DomainModels;
using UnionManager.Models.Repositories;
using UnionManager.ViewModels.Profile;

namespace UnionManager.Controllers
{
    [Authorize(Roles = "Admin, HoleBoss, Boss")]
    public class ProfileController : Controller
    {
        UserRepository blUser = new UserRepository();

        [HttpGet]
        public ActionResult ProfileDetail()
        {
            var user = blUser.Where(p => p.Username == User.Identity.Name).Single();
            string trades = "", vehicles = "";

            int counter = 0;
            int i = 0;
            foreach (var item in user.Relations)
            {
                if (counter != 0) trades += " /";
                trades += item.Trade.Name;

                if (item.VehicleId != null)
                {
                    if (i != 0) vehicles += " /";
                    vehicles += item.Vehicle.Model;
                    i++;
                }
                counter++;
            }

            var model = new ProfileViewModel()
            {
                User = user,
                Trades = (trades == "") ? "-" : trades,
                Vehicles = (vehicles == "") ? "-" : vehicles
            };

            return View(model);
        }

        [HttpGet]
        public ActionResult EditProfile()
        {
            User user = blUser.Where(p => p.Username == User.Identity.Name).Single();
            if (user.BirthDate != null)
            {
                user.BirthDate = Convert.ToDateTime(user.BirthDate).ToPersianDate();
            }

            var model = new ProfileViewModel()
            {
                User = user,
                RealUser = user
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult EditProfile(User user, HttpPostedFileBase UploadImage)
        {
            try
            {
                var existUser = blUser.Where(p => p.Username == User.Identity.Name).Single();

                //////// validation
                if (!Utilities.IsRequiredStringInputsValid(user.Name, user.Family, user.NationalCode))
                {
                    ViewBag.Message1 = "فیلدها را بدرستی وارد کنید";
                    ViewBag.Type = "Error";
                    var model = new ProfileViewModel()
                    {
                        User = user,
                        RealUser = blUser.Where(p => p.Username == User.Identity.Name).Single()
                    };
                    return View(model);
                }

                user.Status = "فعال";
                user.Password = existUser.Password;
                user.ConfirmPassword = existUser.Password;
                user.Name = user.Name.ToFarsiString().Trim();
                user.Family = user.Family.ToFarsiString().Trim();
                user.FatherName = (string.IsNullOrEmpty(user.FatherName) ? null : user.FatherName.ToFarsiString().Trim());
                user.NationalCode = user.NationalCode.ToFarsiString().Trim();
                user.IdNo = (string.IsNullOrEmpty(user.IdNo) ? null : user.IdNo.ToFarsiString().Trim());
                user.Mobile = (string.IsNullOrEmpty(user.Mobile) ? null : user.Mobile.ToFarsiString().Trim());
                user.Tel = (string.IsNullOrEmpty(user.Tel) ? null : user.Tel.ToFarsiString().Trim());
                user.Username = existUser.Username;
                user.RoleId = existUser.RoleId;

                if (user.BirthDate != null)
                {
                    user.BirthDate = Convert.ToDateTime(user.BirthDate).ToMiladiDate();
                }
                /////////////////
                ///////////////////////////// image
                bool deleteOldIamge = false;
                string oldIamge = existUser.Image;
                string imagePath = null;
                string fileName = null;
                if (UploadImage != null)
                {
                    fileName = UploadImage.FileName;
                    if (!fileName.IsImage())
                    {
                        ViewBag.Message1 = "فایل تصویر معتبر نیست";
                        ViewBag.Type = "Error";
                        var model = new ProfileViewModel()
                        {
                            User = user,
                            RealUser = blUser.Where(p => p.Username == User.Identity.Name).Single()
                        };
                        return View(model);
                    }

                    if (oldIamge != null)
                    {
                        deleteOldIamge = true;
                    }
                    ////// وقتی عکسی وجود داشت نام عکس جدید را تغییر دهد و عکس قبلی را حذف کند
                    while (System.IO.File.Exists(Server.MapPath("~") + "Files\\UploadImages\\" + fileName) == true)
                    {
                        string extension = fileName.Substring(fileName.Length - 4);
                        fileName = fileName.Replace(extension, "");
                        fileName += 1.ToString();
                        fileName = fileName + extension;
                    }
                    /////////////
                    imagePath = Server.MapPath("~") + "Files\\UploadImages\\" + fileName;
                    UploadImage.InputStream.ResizeImageByWidth(500, imagePath, Utilities.ImageComperssion.Normal);
                }
                ////////////////////////////

                string message = "";
                if (blUser.Update(user, fileName, oldIamge, out message))
                {
                    if (deleteOldIamge == true)
                    {
                        if (System.IO.File.Exists(Server.MapPath("~") + "Files\\UploadImages\\" + oldIamge))
                        {
                            System.IO.File.Delete(Server.MapPath("~") + "Files\\UploadImages\\" + oldIamge);
                        }
                    }

                    ViewBag.Message1 = "پروفایل با موفقیت ویرایش شد";
                    ViewBag.Type = "Success";
                    var model = new ProfileViewModel()
                    {
                        User = user,
                        RealUser = blUser.Where(p => p.Username == User.Identity.Name).Single()
                    };
                    return View(model);
                }
                else
                {
                    if (fileName != null)
                    {
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }

                    ViewBag.Message1 = "ویرایش پروفایل با خطا همراه شده است";
                    ViewBag.Type = "Error";
                    var model = new ProfileViewModel()
                    {
                        User = user,
                        RealUser = blUser.Where(p => p.Username == User.Identity.Name).Single()
                    };
                    return View(model);
                }
            }
            catch
            {
                ViewBag.Message1 = "ویرایش پروفایل با خطا همراه شده است";
                ViewBag.Type = "Error";
                var model = new ProfileViewModel()
                {
                    User = user,
                    RealUser = blUser.Where(p => p.Username == User.Identity.Name).Single()
                };
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult ChangePassword(string oldPass, string newPass, string repeatNewPass)
        {
            try
            {
                var user = blUser.Where(p => p.Username == User.Identity.Name).Single();
                var model = new ProfileViewModel()
                {
                    User = user,
                    RealUser = user
                };

                if (string.IsNullOrEmpty(oldPass))
                {
                    ViewBag.Message2 = "کلمه عبور را وارد کنید";
                    ViewBag.Type2 = "Error";
                    return View("EditProfile", model);
                }

                if (string.IsNullOrEmpty(newPass) || string.IsNullOrEmpty(repeatNewPass))
                {
                    ViewBag.Message2 = "کلمه عبور جدید را وارد کنید";
                    ViewBag.Type2 = "Error";
                    return View("EditProfile", model);
                }

                if (newPass != repeatNewPass)
                {
                    ViewBag.Message2 = "کلمه عبور جدید را بدرستی تکرار کنید";
                    ViewBag.Type2 = "Error";
                    return View("EditProfile", model);
                }

                if (oldPass.Encrypt() != user.Password)
                {
                    ViewBag.Message2 = "کلمه عبور اشتباه است";
                    ViewBag.Type2 = "Error";
                    return View("EditProfile", model);
                }

                user.Password = newPass.Encrypt();
                user.ConfirmPassword = user.Password;

                string message = "";
                if (blUser.Update(user, null, user.Image, out message))
                {
                    ViewBag.Message2 = "کلمه عبور با موفقیت ویرایش شد";
                    ViewBag.Type2 = "Success";
                    return View("EditProfile", model);
                }
                else
                {
                    ViewBag.Message2 = "خطا رخ داده است. مجددا تلاش کنید";
                    ViewBag.Type2 = "Error";
                    return View("EditProfile", model);
                }
            }
            catch
            {
                return RedirectToAction("Error", "Home", new { message = "خطا رخ داده است. مجددا تلاش کنید" });
            }
        }
    }
}