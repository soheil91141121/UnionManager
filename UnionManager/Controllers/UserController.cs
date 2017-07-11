using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using UnionManager.Models.DomainModels;
using UnionManager.Models.ExcelModels;
using UnionManager.Models.Repositories;
using UnionManager.ViewModels.User;

namespace UnionManager.Controllers
{
    [Authorize(Roles = "Admin, HoleBoss")]
    public class UserController : Controller
    {
        UnionManager.Helpers.Utilities.SessionHelper sh = new Helpers.Utilities.SessionHelper();

        UserRepository blUser = new UserRepository();
        RoleRepository blRole = new RoleRepository();
        RelationRepository blRelation = new RelationRepository();

        [HttpGet]
        public ActionResult Users(int page = 1)
        {
            try
            {
                if (Convert.ToBoolean(Session["isUser"]) == false)
                {
                    Session["User_Name"] = null;
                    Session["User_Family"] = null;
                    Session["User_RoleId"] = null;
                    Session["User_FatherName"] = null;
                    Session["User_Gender"] = null;
                    Session["User_NationalCode"] = null;
                    Session["User_IdNo"] = null;
                    Session["User_Mobile"] = null;
                    Session["User_Tel"] = null;
                    Session["User_Username"] = null;
                    Session["User_IsInsuranced"] = null;
                    Session["User_Status"] = null;
                }

                long totalModelCount = 0;
                var users = Search(ref totalModelCount,
                                      (Session["User_Name"] == null) ? null : Session["User_Name"].ToString(),
                                      (Session["User_Family"] == null) ? null : Session["User_Family"].ToString(),
                                      (Session["User_RoleId"] == null) ? 0 : Convert.ToInt64(Session["User_RoleId"]),
                                      (Session["User_FatherName"] == null) ? null : Session["User_FatherName"].ToString(),
                                      (Session["User_Gender"] == null) ? -1 : Convert.ToInt32(Session["User_Gender"]),
                                      (Session["User_NationalCode"] == null) ? null : Session["User_NationalCode"].ToString(),
                                      (Session["User_IdNo"] == null) ? null : Session["User_IdNo"].ToString(),
                                      (Session["User_Mobile"] == null) ? null : Session["User_Mobile"].ToString(),
                                      (Session["User_Tel"] == null) ? null : Session["User_Tel"].ToString(),
                                      (Session["User_Username"] == null) ? null : Session["User_Username"].ToString(),
                                      (Session["User_IsInsuranced"] == null) ? -1 : Convert.ToInt32(Session["User_IsInsuranced"]),
                                      (Session["User_Status"] == null) ? -1 : Convert.ToInt32(Session["User_Status"]));

                int totalItemCount = users.Count();
                float totalPageCount = (float)Math.Ceiling((float)totalItemCount / 10);
                var model = new UserViewModel()
                {
                    Users = users.OrderBy(p => p.Id).Skip((page - 1) * 10).Take(10).ToList(),
                    Roles = (User.IsInRole("Admin") ? blRole.Select().ToList() : blRole.Where(p => p.RoleName != "Admin" && p.RoleName != "HoleBoss").ToList()),
                    CurrentPage = page,
                    TotalPageCount = ((int)totalPageCount == 0) ? 1 : (int)totalPageCount,
                    TotalItemCount = totalItemCount,
                    TotalModelCount = totalModelCount
                };

                return View(model);
            }
            catch
            {
                return RedirectToAction("Error404", "Home");
            }
        }

        [HttpGet]
        public ActionResult AddUser()
        {
            var model = new AddUserViewModel()
            {
                Roles = (User.IsInRole("Admin") ? blRole.Select().ToList() : blRole.Where(p => p.RoleName != "Admin" && p.RoleName != "HoleBoss").ToList()),
                User = new User { Gender = true }
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult AddUser(User user, HttpPostedFileBase UploadImage)
        {
            try
            {
                //////// validation
                if (!Utilities.IsRequiredStringInputsValid(user.Name, user.Family, user.NationalCode, user.Username, user.Password))
                {
                    return RedirectToAction("Error", "Home", new { message = "فیلدهای ستاره دار را بدرستی وارد کنید" });
                }

                user.Status = "فعال";
                user.Password = user.Password.Encrypt();
                user.ConfirmPassword = user.Password;
                user.Name = user.Name.ToFarsiString().Trim();
                user.Family = user.Family.ToFarsiString().Trim();
                user.FatherName = (string.IsNullOrEmpty(user.FatherName) ? null : user.FatherName.ToFarsiString().Trim());
                user.NationalCode = user.NationalCode.ToFarsiString().Trim();
                user.IdNo = (string.IsNullOrEmpty(user.IdNo) ? null : user.IdNo.ToFarsiString().Trim());
                user.Mobile = (string.IsNullOrEmpty(user.Mobile) ? null : user.Mobile.ToFarsiString().Trim());
                user.Tel = (string.IsNullOrEmpty(user.Tel) ? null : user.Tel.ToFarsiString().Trim());
                user.Username = user.Username.ToFarsiString().Trim();

                if (user.BirthDate != null)
                {
                    user.BirthDate = Convert.ToDateTime(user.BirthDate).ToMiladiDate();
                }
                //////////////

                if (ModelState.IsValid)
                {
                    string imagePath = null;
                    if (UploadImage != null)
                    {
                        string fileName = UploadImage.FileName;
                        if (!fileName.IsImage())
                        {
                            return RedirectToAction("Error", "Home", new { message = "فایل تصویر معتبر نیست" });
                        }

                        ////// وقتی عکسی وجود داشت نام عکس جدید را تغییر دهد
                        while (System.IO.File.Exists(Server.MapPath("~") + "Files\\UploadImages\\" + fileName) == true)
                        {
                            string extension = fileName.Substring(fileName.Length - 4);
                            fileName = fileName.Replace(extension, "");
                            fileName += 1.ToString();
                            fileName = fileName + extension;
                        }
                        /////////////
                        user.Image = fileName;
                        imagePath = Server.MapPath("~") + "Files\\UploadImages\\" + fileName;
                        UploadImage.InputStream.ResizeImageByWidth(500, imagePath, Utilities.ImageComperssion.Normal);
                    }

                    string message = "";
                    if (blUser.Add(user, out message))
                    {
                        return RedirectToAction("Users", "User");
                    }
                    else
                    {
                        if (imagePath != null)
                        {
                            System.IO.File.Delete(imagePath);
                        }

                        if (message.Contains("Users(Username Must Unique)"))
                        {
                            return RedirectToAction("Error", "Home", new { message = "نام کاربری باید یکتا باشد" });
                        }

                        return RedirectToAction("Error", "Home", new { message = "خطا رخ داده است. مجددا تلاش کنید" });
                    }
                }
                else
                {
                    return RedirectToAction("Error", "Home", new { message = "خطا رخ داده است. مجددا تلاش کنید" });
                }
            }
            catch
            {
                return RedirectToAction("Error", "Home", new { message = "خطا رخ داده است. مجددا تلاش کنید" });
            }
        }

        [HttpGet]
        public ActionResult EditUser(long? id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("Error404", "Home");
                }

                User user = blUser.Find(Convert.ToInt64(id));
                if (user.BirthDate != null)
                {
                    user.BirthDate = Convert.ToDateTime(user.BirthDate).ToPersianDate();
                }

                var model = new AddUserViewModel()
                {
                    Roles = (User.IsInRole("Admin") ? blRole.Select().ToList() : blRole.Where(p => p.RoleName != "Admin" && p.RoleName != "HoleBoss").ToList()),
                    User = user
                };

                return View(model);
            }
            catch
            {
                return RedirectToAction("Error404", "Home");
            }
        }

        [HttpPost]
        public ActionResult EditUser(User user, HttpPostedFileBase UploadImage)
        {
            try
            {
                //////// validation
                if (!Utilities.IsRequiredStringInputsValid(user.Name, user.Family, user.NationalCode, user.Username, user.Password))
                {
                    return RedirectToAction("Error", "Home", new { message = "فیلدهای ستاره دار را بدرستی وارد کنید" });
                }

                user.Status = "فعال";
                user.ConfirmPassword = user.Password;
                user.Name = user.Name.ToFarsiString().Trim();
                user.Family = user.Family.ToFarsiString().Trim();
                user.FatherName = (string.IsNullOrEmpty(user.FatherName) ? null : user.FatherName.ToFarsiString().Trim());
                user.NationalCode = user.NationalCode.ToFarsiString().Trim();
                user.IdNo = (string.IsNullOrEmpty(user.IdNo) ? null : user.IdNo.ToFarsiString().Trim());
                user.Mobile = (string.IsNullOrEmpty(user.Mobile) ? null : user.Mobile.ToFarsiString().Trim());
                user.Tel = (string.IsNullOrEmpty(user.Tel) ? null : user.Tel.ToFarsiString().Trim());
                user.Username = user.Username.ToFarsiString().Trim();

                if (user.BirthDate != null)
                {
                    user.BirthDate = Convert.ToDateTime(user.BirthDate).ToMiladiDate();
                }
                //////////////

                bool deleteOldIamge = false;
                string oldIamge = blUser.Find(user.Id).Image;
                string imagePath = null;
                string fileName = null;
                if (UploadImage != null)
                {
                    fileName = UploadImage.FileName;
                    if (!fileName.IsImage())
                    {
                        return RedirectToAction("Error", "Home", new { message = "فایل تصویر معتبر نیست" });
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

                    return RedirectToAction("Users", "User");
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

                    if (message.Contains("Users(Username Must Unique)"))
                    {
                        return RedirectToAction("Error", "Home", new { message = "نام کاربری باید یکتا باشد" });
                    }

                    return RedirectToAction("Error", "Home", new { message = "خطا رخ داده است. مجددا تلاش کنید" });
                }
            }
            catch
            {
                return RedirectToAction("Error", "Home", new { message = "خطا رخ داده است. مجددا تلاش کنید" });
            }
        }

        [HttpPost]
        public ActionResult DeleteUser(long? id)
        {
            try
            {
                if (id == null) return RedirectToAction("Error404", "Home");

                string image = blUser.Find(Convert.ToInt64(id)).Image;

                string message = "";
                if (blUser.Delete(Convert.ToInt64(id), out message))
                {
                    if (image != null)
                    {
                        if (System.IO.File.Exists(Server.MapPath("~") + "Files\\UploadImages\\" + image) == true)
                        {
                            System.IO.File.Delete(Server.MapPath("~") + "Files\\UploadImages\\" + image);
                        }
                    }

                    return Json(new JsonData()
                    {
                        Success = true,
                        Script = MessageBox.Show("عملیات حذف با موفقیت انجام شد", MessageType.Success).Script,
                        Html = ""
                    });
                }
                else
                {
                    if (message.Contains("FK_Activity_Users") || message.Contains("FK_Users_Roles") || message.Contains("FK_UserTrade_Users") || message.Contains("FK_UserVehicle_Users"))
                    {
                        return Json(new JsonData()
                        {
                            Success = false,
                            Script = MessageBox.Show("این مقدار در سیستم بکار رفته است. امکان حذف وجود ندارد", MessageType.Error).Script,
                            Html = ""
                        });
                    }

                    return Json(new JsonData()
                    {
                        Success = false,
                        Script = MessageBox.Show("خطا رخ داده است. مجددا تلاش کنید", MessageType.Error).Script,
                        Html = ""
                    });
                }
            }
            catch
            {
                return Json(new JsonData()
                {
                    Success = false,
                    Script = MessageBox.Show("خطا رخ داده است. مجددا تلاش کنید", MessageType.Error).Script,
                    Html = ""
                });
            }
        }

        [HttpPost]
        public ActionResult ChangePassword(long? userId, string password, string confirmPassword)
        {
            try
            {
                if (userId == null)
                {
                    return Json(new JsonData()
                    {
                        Success = false,
                        Script = MessageBox.Show("خطا رخ داده است. مجددا تلاش کنید", MessageType.Error).Script,
                        Html = ""
                    });
                }
                if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
                {
                    return Json(new JsonData()
                    {
                        Success = false,
                        Script = MessageBox.Show("مقادیر را وارد کنید", MessageType.Error).Script,
                        Html = ""
                    });
                }
                if (password != confirmPassword)
                {
                    return Json(new JsonData()
                    {
                        Success = false,
                        Script = MessageBox.Show("کلمه عبور را بدرستی تکرار کنید", MessageType.Error).Script,
                        Html = ""
                    });
                }
                if (password.Length > 50)
                {
                    return Json(new JsonData()
                    {
                        Success = false,
                        Script = MessageBox.Show("کلمه عبور باید حداکثر 50 کاراکتر باشد", MessageType.Error).Script,
                        Html = ""
                    });
                }

                var user = blUser.Find(Convert.ToInt64(userId));
                user.Password = password.Encrypt();
                user.ConfirmPassword = user.Password;

                string message = "";
                if (blUser.Update(user, null, null, out message))
                {
                    return Json(new JsonData()
                    {
                        Success = true,
                        Script = MessageBox.Show("کلمه عبور با موفقیت تغییر یافت", MessageType.Success).Script,
                        Html = ""
                    });
                }
                else
                {
                    return Json(new JsonData()
                    {
                        Success = false,
                        Script = MessageBox.Show("خطا رخ داده است. مجددا تلاش کنید", MessageType.Error).Script,
                        Html = ""
                    });
                }
            }
            catch
            {
                return Json(new JsonData()
                {
                    Success = false,
                    Script = MessageBox.Show("خطا رخ داده است. مجددا تلاش کنید", MessageType.Error).Script,
                    Html = ""
                });
            }
        }

        [HttpPost]
        public JsonResult GetUsername(long? id)
        {
            try
            {
                if (id == null) return Json(new { Success = false, username = "" });
                return Json(new { Success = true, username = blUser.Find(Convert.ToInt64(id)).Username });
            }
            catch
            {
                return Json(new { Success = false, username = "" });
            }
        }

        //-------------------------------- search -------------------------------------
        [HttpGet]
        public ActionResult SearchUser(string Name, string Family, long? RoleId, string FatherName, int? Gender, string NationalCode, string IdNo, string Mobile, string Tel, string Username, int? IsInsuranced, int? Status)
        {
            Session["User_Name"] = Name;
            Session["User_Family"] = Family;
            Session["User_RoleId"] = RoleId;
            Session["User_FatherName"] = FatherName;
            Session["User_Gender"] = Gender;
            Session["User_NationalCode"] = NationalCode;
            Session["User_IdNo"] = IdNo;
            Session["User_Mobile"] = Mobile;
            Session["User_Tel"] = Tel;
            Session["User_Username"] = Username;
            Session["User_IsInsuranced"] = IsInsuranced;
            Session["User_Status"] = Status;
            Session["isUser"] = true;

            return RedirectToAction("Users", "User");
        }

        public List<User> Search(ref long totalModelCount, string Name, string Family, long? RoleId, string FatherName, int? Gender, string NationalCode, string IdNo, string Mobile, string Tel, string Username, int? IsInsuranced, int? Status)
        {
            var Users = (User.IsInRole("Admin") ? blUser.Select().ToList() : blUser.Where(p => p.Role.RoleName != "Admin" && p.Role.RoleName != "HoleBoss").ToList());
            totalModelCount = Users.Count();

            if (!string.IsNullOrEmpty(Name))
            {
                Users = Users.Where(p => p.Name.Contains(Name.ToFarsiString().Trim())).ToList();
            }
            if (!string.IsNullOrEmpty(Family))
            {
                Users = Users.Where(p => p.Family.Contains(Family.ToFarsiString().Trim())).ToList();
            }
            if (RoleId != null && RoleId > 0)
            {
                Users = Users.Where(p => p.RoleId == RoleId).ToList();
            }
            if (!string.IsNullOrEmpty(FatherName))
            {
                var notNullList = Users.Where(p => p.FatherName != null).ToList();
                Users = notNullList.Where(p => p.FatherName.Contains(FatherName.ToFarsiString().Trim())).ToList();
            }
            if (Gender != null && Gender >= 0)
            {
                if (Gender == 1)
                    Users = Users.Where(p => p.Gender == true).ToList();
                else
                    Users = Users.Where(p => p.Gender == false).ToList();
            }
            if (!string.IsNullOrEmpty(NationalCode))
            {
                Users = Users.Where(p => p.NationalCode.Contains(NationalCode.ToFarsiString().Trim())).ToList();
            }
            if (!string.IsNullOrEmpty(IdNo))
            {
                var notNullList = Users.Where(p => p.IdNo != null).ToList();
                Users = notNullList.Where(p => p.IdNo.Contains(IdNo.ToFarsiString().Trim())).ToList();
            }
            if (!string.IsNullOrEmpty(Mobile))
            {
                var notNullList = Users.Where(p => p.Mobile != null).ToList();
                Users = notNullList.Where(p => p.Mobile.Contains(Mobile.ToFarsiString().Trim())).ToList();
            }
            if (!string.IsNullOrEmpty(Tel))
            {
                var notNullList = Users.Where(p => p.Tel != null).ToList();
                Users = notNullList.Where(p => p.Tel.Contains(Tel.ToFarsiString().Trim())).ToList();
            }
            if (!string.IsNullOrEmpty(Username))
            {
                Users = Users.Where(p => p.Username.Contains(Username.ToFarsiString().Trim())).ToList();
            }
            if (IsInsuranced != null && IsInsuranced >= 0)
            {
                if (IsInsuranced == 1)
                    Users = Users.Where(p => p.IsInsuranced == true).ToList();
                else
                    Users = Users.Where(p => p.IsInsuranced == false).ToList();
            }
            if (Status != null && Status >= 0)
            {
                if (Status == 1)
                    Users = Users.Where(p => p.Status == "فعال").ToList();
                else
                    Users = Users.Where(p => p.Status == "غیرفعال").ToList();
            }

            return Users;
        }

        //-------------------------------- detail -------------------------------------
        [HttpGet]
        public ActionResult UserDetail(long? id)
        {
            try
            {
                if (id != null)
                {
                    var relation = blRelation.Where(p => p.UserId == id).ToList();
                    string trades = "", vehicles = "";

                    int counter = 0;
                    foreach (var item in relation)
                    {
                        if (counter != 0) trades += " /";
                        trades += item.Trade.Name;
                        counter++;
                    }

                    int i = 0;
                    int j = 0;
                    foreach (var item in relation)
                    {
                        if (item.VehicleId != null)
                        {
                            if (j != 0) vehicles += " /";
                            vehicles += item.Vehicle.Model;
                            j++;
                        }
                        i++;
                    }

                    var model = new UserDetailViewModel()
                    {
                        User = blUser.Find(Convert.ToInt64(id)),
                        UserVehicles = (vehicles == "") ? "-" : vehicles,
                        UserTrades = (trades == "") ? "-" : trades
                    };

                    return View(model);
                }
                else
                {
                    return RedirectToAction("Error404", "Home");
                }
            }
            catch
            {
                return RedirectToAction("Error404", "Home");
            }
        }

        //-------------------------------- excel----------------------------------
        private List<UserExcelModel> GetExcelModel()
        {
            long totalModelCount = 0;
            var users = Search(ref totalModelCount,
                                  (Session["User_Name"] == null) ? null : Session["User_Name"].ToString(),
                                  (Session["User_Family"] == null) ? null : Session["User_Family"].ToString(),
                                  (Session["User_RoleId"] == null) ? 0 : Convert.ToInt64(Session["User_RoleId"]),
                                  (Session["User_FatherName"] == null) ? null : Session["User_FatherName"].ToString(),
                                  (Session["User_Gender"] == null) ? -1 : Convert.ToInt32(Session["User_Gender"]),
                                  (Session["User_NationalCode"] == null) ? null : Session["User_NationalCode"].ToString(),
                                  (Session["User_IdNo"] == null) ? null : Session["User_IdNo"].ToString(),
                                  (Session["User_Mobile"] == null) ? null : Session["User_Mobile"].ToString(),
                                  (Session["User_Tel"] == null) ? null : Session["User_Tel"].ToString(),
                                  (Session["User_Username"] == null) ? null : Session["User_Username"].ToString(),
                                  (Session["User_IsInsuranced"] == null) ? -1 : Convert.ToInt32(Session["User_IsInsuranced"]),
                                  (Session["User_Status"] == null) ? -1 : Convert.ToInt32(Session["User_Status"]));

            var model = new List<UserExcelModel>();
            UserExcelModel excelModel;

            foreach (var item in users)
            {
                excelModel = new UserExcelModel();
                excelModel.Name = item.Name;
                excelModel.Family = item.Family;
                excelModel.RoleName = item.Role.Name;
                excelModel.Gender = (item.Gender == true) ? "مرد" : "زن";
                excelModel.FatherName = (string.IsNullOrEmpty(item.FatherName)) ? "-" : item.FatherName;
                excelModel.NationalCode = item.NationalCode;
                excelModel.IdNo = (string.IsNullOrEmpty(item.IdNo)) ? "-" : item.IdNo;
                excelModel.BirthDate = (item.BirthDate == null) ? "-" : Convert.ToDateTime(item.BirthDate).ToPersianDateString();
                excelModel.IsInsuranced = (item.IsInsuranced == true) ? "بله" : "خیر";
                excelModel.Mobile = (string.IsNullOrEmpty(item.Mobile)) ? "-" : item.Mobile;
                excelModel.Tel = (string.IsNullOrEmpty(item.Tel)) ? "-" : item.Tel;
                excelModel.Username = item.Username;
                excelModel.Status = item.Status;

                var trades = blRelation.Where(p => p.UserId == item.Id).ToList();
                if (trades.Count() != 0)
                {
                    int counter = 0;
                    foreach (var t in trades)
                    {
                        if (counter == 0)
                            excelModel.TradesName = t.Trade.Name;
                        else
                            excelModel.TradesName += " / " + t.Trade.Name;

                        counter++;
                    }
                }
                else
                {
                    excelModel.TradesName = "-";
                }

                var vehicles = blRelation.Where(p => p.UserId == item.Id).ToList();
                if (vehicles.Count() != 0)
                {
                    int counter = 0;
                    int i = 0;
                    foreach (var v in vehicles)
                    {
                        if (v.VehicleId != null)
                        {
                            if (i == 0)
                                excelModel.VehiclesName = v.Vehicle.Model;
                            else
                                excelModel.VehiclesName += " / " + v.Vehicle.Model;
                            i++;
                        }

                        counter++;
                    }
                }
                else
                {
                    excelModel.VehiclesName = "-";
                }

                model.Add(excelModel);
            }

            return model;
        }

        public ActionResult ExportToExcel()
        {

            string output = string.Empty;

            var model = GetExcelModel();
            if (model.Count() == 0)
            {
                return RedirectToAction("Users", "User");
            }

            output += @"<html dir='rtl'><head><meta http-equiv=Content-Type content=""text/html; charset=utf-8""><style> .text { mso-number-format:\@; } .trFormat{color:Black;background-color:#DEDFDE;} </style></head><table dir=""rtl"" cellspacing=""1"" cellpadding=""3"" border=""0"" style=""background-color:White;border-color:White;border-width:2px;border-style:Ridge;font-family:Tahoma;font-size:11px;width:100%;""><tbody>";
            output += @"<tr><th class='text-center'>نام</th><th class='text-center'>نام خانوادگی</th><th class='text-center'>نقش</th><th class='text-center'>جنسیت</th><th class='text-center'>نام پدر</th><th class='text-center'>کد ملی</th><th class='text-center'>شماره شناسنامه</th><th class='text-center'>تاریخ تولد</th><th class='text-center'>بیمه شده</th><th class='text-center'>شماره موبایل</th><th class='text-center'>تلفن ثابت</th><th class='text-center'>نام کاربری</th><th class='text-center'>وضعیت</th><th class='text-center'>اصناف</th><th class='text-center'>وسایل نقلیه</th></tr>";

            foreach (var item in model)
            {
                output += @"<tr><td align='center'>" + item.Name + "</td><td align='center'>" + item.Family + "</td><td align='center'>" + item.RoleName + "</td><td align='center'>" + item.Gender + "</td><td align='center'>" + item.FatherName + "</td><td align='center'>" + item.NationalCode + "</td><td align='center'>" + item.IdNo + "</td><td align='center'>" + item.BirthDate + "</td><td align='center'>" + item.IsInsuranced + "</td><td align='center'>" + item.Mobile + "</td><td align='center'>" + item.Tel + "</td><td align='center'>" + item.Username + "</td><td align='center'>" + item.Status + "</td><td align='center'>" + item.TradesName + "</td><td align='center'>" + item.VehiclesName + "</td></tr>";
            }

            output += "</tbody></table></html>";
            Response.Clear();
            Response.ContentEncoding = Encoding.UTF8;
            Response.ContentType = @"application/vnd.xls;";
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}.xls", "Users_" + DateTime.Now.ToPersianDateString()));
            Response.Write(output);
            Response.End();

            return RedirectToAction("Users", "User");
        }
    }
}