using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UnionManager.Models.DomainModels;
using UnionManager.Models.Repositories;
using UnionManager.ViewModels.Boss;

namespace UnionManager.Controllers
{
    [Authorize(Roles = "Boss")]
    public class BossController : Controller
    {
        UnionManager.Helpers.Utilities.SessionHelper sh = new Helpers.Utilities.SessionHelper();

        RelationRepository blRelation = new RelationRepository();
        UserRepository blUser = new UserRepository();
        TradeRepository blTrade = new TradeRepository();
        VehicleRepository blVehicle = new VehicleRepository();
        VehicleGroupRepository blVehicleGroup = new VehicleGroupRepository();
        ColorRepository blColor = new ColorRepository();

        [HttpGet]
        public ActionResult Details()
        {
            try
            {
                var user = blUser.Where(p => p.Username == User.Identity.Name).Single();
                bool isRelation = blRelation.Where(p => p.UserId == user.Id).Any();

                if (!isRelation)
                {
                    var model = new BossViewModel()
                    {
                        Trade = null
                    };

                    return View(model);
                }
                else
                {
                    var tradeId = blRelation.Where(p => p.UserId == user.Id).Single().TradeId;
                    var model = new BossViewModel()
                    {
                        Trade = blTrade.Where(p => p.Id == tradeId).Single(),
                        Relations = blRelation.Where(p => p.TradeId == tradeId && p.UserId != user.Id).ToList(),
                        VehicleGroups = blVehicleGroup.Where(p => p.Status == "فعال").ToList(),
                        Colors = blColor.Where(p => p.Status == "فعال").ToList(),
                        User = new User() { Gender = true, Password = "12345", ConfirmPassword = "12345", RoleId = 5 }
                    };

                    return View(model);
                }
            }
            catch
            {
                return RedirectToAction("Error404", "Home");
            }
        }

        //---------------- user ------------------
        [HttpPost]
        public JsonResult AddUserBoss(User user, HttpPostedFileBase UploadImage)
        {
            try
            {
                //////// validation
                if (!Utilities.IsRequiredStringInputsValid(user.Name, user.Family, user.NationalCode, user.Username, user.Password))
                {
                    return Json(new JsonData()
                    {
                        Success = false,
                        Script = MessageBox.Show("فیلدهای ستاره دار را بدرستی وارد کنید", MessageType.Error).Script,
                        Html = ""
                    });
                }

                user.Status = "فعال";
                user.RoleId = 5;
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
                    string message = "";
                    if (blUser.Add(user, out message))
                    {
                        return Json(new
                        {
                            UserId = blUser.Where(p => p.Username == user.Username).Single().Id,
                            Success = true,
                        });
                    }
                    else
                    {
                        if (message.Contains("Users(Username Must Unique)"))
                        {
                            return Json(new JsonData()
                            {
                                Success = false,
                                Script = MessageBox.Show("نام کاربری باید یکتا باشد", MessageType.Error).Script,
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
        public JsonResult AddRelationForUser(long userId, long tradeId)
        {
            try
            {
                User user = blUser.Find(userId);
                user.ConfirmPassword = user.Password;
                var relation = new Relation() { UserId = userId, TradeId = tradeId, Status = "تمام وقت" };
                string message = "";
                if (blRelation.Add(relation, out message))
                {
                    Session["User_Id_Boss"] = userId;
                    Session["Relation_Id_Boss"] = blRelation.Where(p => p.UserId == userId && p.TradeId == tradeId).Single().Id;
                    return Json(new { Success = true });
                }
                else
                {
                    string message2 = "";
                    blUser.Delete(user, out message2);
                    if (user.Image != null)
                    {
                        if (System.IO.File.Exists(Server.MapPath("~") + "Files\\UploadImages\\" + user.Image))
                        {
                            System.IO.File.Delete(Server.MapPath("~") + "Files\\UploadImages\\" + user.Image);
                        }
                    }

                    if (message.Contains("Relations(TradeId And UserId Must Unique)"))
                    {
                        return Json(new JsonData()
                        {
                            Success = false,
                            Script = MessageBox.Show("این کاربر در این صنف وجود دارد. امکان درج وجود ندارد", MessageType.Error).Script,
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
        public ActionResult UploadUserImage()
        {
            try
            {
                if (Request.ContentLength < 70)
                {
                    return Json(new JsonData()
                    {
                        Success = true,
                        Script = MessageBox.Show("عملیات درج با موفقیت انجام شد", MessageType.Success).Script,
                        Html = ""
                    });
                }
                else
                {
                    long userId = Convert.ToInt64(Session["User_Id_Boss"]);
                    long relationId = Convert.ToInt64(Session["Relation_Id_Boss"]);

                    var user = blUser.Find(userId);
                    user.ConfirmPassword = user.Password;

                    var UploadImage = Request.Files[0];

                    string imagePath = null;
                    string fileName = UploadImage.FileName;
                    if (!fileName.IsImage())
                    {
                        string message = "";
                        blUser.Delete(userId, out message);
                        string message2 = "";
                        blRelation.Delete(relationId, out message2);

                        return Json(new JsonData()
                        {
                            Success = false,
                            Script = MessageBox.Show("فایل تصویر معتبر نیست", MessageType.Error).Script,
                            Html = ""
                        });
                    }

                    // وقتی عکسی وجود داشت نام عکس جدید را تغییر دهد
                    while (System.IO.File.Exists(Server.MapPath("~") + "Files\\UploadImages\\" + fileName) == true)
                    {
                        string extension = fileName.Substring(fileName.Length - 4);
                        fileName = fileName.Replace(extension, "");
                        fileName += 1.ToString();
                        fileName = fileName + extension;
                    }
                    //
                    user.Image = fileName;
                    imagePath = Server.MapPath("~") + "Files\\UploadImages\\" + fileName;
                    UploadImage.InputStream.ResizeImageByWidth(500, imagePath, Utilities.ImageComperssion.Normal);

                    string message3 = "";
                    if (blUser.Update(user, fileName, null, out message3))
                    {
                        return Json(new JsonData()
                        {
                            Success = true,
                            Script = MessageBox.Show("عملیات درج با موفقیت انجام شد", MessageType.Success).Script,
                            Html = ""
                        });
                    }
                    else
                    {
                        string message = "";
                        blUser.Delete(user, out message);
                        if (user.Image != null)
                        {
                            if (System.IO.File.Exists(Server.MapPath("~") + "Files\\UploadImages\\" + user.Image))
                            {
                                System.IO.File.Delete(Server.MapPath("~") + "Files\\UploadImages\\" + user.Image);
                            }
                        }

                        return Json(new JsonData()
                        {
                            Success = false,
                            Script = MessageBox.Show("فایل تصویر آپلود نشده است", MessageType.Error).Script,
                            Html = ""
                        });
                    }
                }
            }
            catch
            {
                string message = "";
                blUser.Delete(Convert.ToInt64(Session["User_Id_Boss"]), out message);
                string message2 = "";
                blRelation.Delete(Convert.ToInt64(Session["Relation_Id_Boss"]), out message2);

                return Json(new JsonData()
                {
                    Success = false,
                    Script = MessageBox.Show("در آپلود تصویر مشکل پیش آمده است. مجددا تلاش کنید", MessageType.Error).Script,
                    Html = ""
                });
            }

        }

        //---------------- vehicle ------------------
        [HttpPost]
        public JsonResult AddVehicleBoss(Vehicle vehicle, string np_FirstNum, string np_Letter, string np_SecondNum, string np_ThirdNum)
        {
            try
            {
                //////// validation
                if (!Utilities.IsRequiredStringInputsValid(vehicle.Model, vehicle.VINName))
                {
                    return Json(new JsonData()
                    {
                        Success = false,
                        Script = MessageBox.Show("فیلدهای ستاره دار را بدرستی وارد کنید", MessageType.Error).Script,
                        Html = ""
                    });
                }

                string numberPlates = GetNumberPlates(np_FirstNum, np_Letter, np_SecondNum, np_ThirdNum);
                if (numberPlates == null)
                {
                    return Json(new JsonData()
                    {
                        Success = false,
                        Script = MessageBox.Show("شماره پلاک را بدرستی وارد کنید", MessageType.Error).Script,
                        Html = ""
                    });
                }

                vehicle.Status = "فعال";
                vehicle.NumberPlates = numberPlates;
                vehicle.Model = vehicle.Model.ToFarsiString().Trim();
                vehicle.VINName = vehicle.VINName.ToFarsiString().Trim();
                //////////////

                if (ModelState.IsValid)
                {
                    string message = "";
                    if (blVehicle.Add(vehicle, out message))
                    {
                        return Json(new
                        {
                            VehicleId = blVehicle.Where(p => p.VINName == vehicle.VINName).Single().Id,
                            Success = true,
                        });
                    }
                    else
                    {
                        if (message.Contains("Vehicles(NumberPlates Must Unique)"))
                        {
                            return Json(new JsonData()
                            {
                                Success = false,
                                Script = MessageBox.Show("شماره پلاک باید یکتا باشد", MessageType.Error).Script,
                                Html = ""
                            });
                        }

                        if (message.Contains("Vehicles(VINName Must Unique)"))
                        {
                            return Json(new JsonData()
                            {
                                Success = false,
                                Script = MessageBox.Show("نام وین باید یکتا باشد", MessageType.Error).Script,
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
        public JsonResult AddRelationForVehicle(long vehicleId, long relationId)
        {
            try
            {
                var relation = blRelation.Find(relationId);
                relation.VehicleId = vehicleId;
                string message = "";
                if (blRelation.Update(relation, out message))
                {
                    return Json(new JsonData()
                    {
                        Success = true,
                        Script = MessageBox.Show("عملیات درج با موفقیت انجام شد", MessageType.Success).Script,
                        Html = ""
                    });
                }
                else
                {
                    var vehicle = blVehicle.Find(vehicleId);
                    string message2 = "";
                    blVehicle.Delete(vehicle, out message2);

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
        public JsonResult ResetVehicle(long relationId)
        {
            try
            {
                var relation = blRelation.Find(relationId);
                relation.VehicleId = null;
                string message = "";
                if (blRelation.Update(relation, out message))
                {
                    return Json(new JsonData()
                    {
                        Success = true,
                        Script = MessageBox.Show("عملیات حذف با موفقیت انجام شد", MessageType.Success).Script,
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

        public string GetNumberPlates(string firstNum, string letter, string secondNum, string thirdNum)
        {
            try
            {
                //// validation
                if (string.IsNullOrEmpty(firstNum) || string.IsNullOrEmpty(letter) || string.IsNullOrEmpty(secondNum) || string.IsNullOrEmpty(thirdNum) || !firstNum.IsNumeric() || !secondNum.IsNumeric() || !thirdNum.IsNumeric())
                {
                    return null;
                }

                int first = Convert.ToInt32(firstNum);
                int second = Convert.ToInt32(secondNum);
                int third = Convert.ToInt32(thirdNum);

                if (first < 10 || first > 99 || second < 100 || second > 999 || third < 10 || third > 99)
                {
                    return null;
                }
                /////////

                return firstNum + letter + secondNum + "|" + thirdNum;
            }
            catch
            {
                return null;
            }
        }

        //---------------- relation ------------------
        [HttpPost]
        public JsonResult DeleteRelation(long? id)
        {
            try
            {
                if (id == null)
                {
                    return Json(new JsonData()
                    {
                        Success = false,
                        Script = MessageBox.Show("خطا رخ داده است. مجددا تلاش کنید", MessageType.Error).Script,
                        Html = ""
                    });
                }

                string message = "";
                if (blRelation.Delete(Convert.ToInt64(id), out message))
                {
                    return Json(new JsonData()
                    {
                        Success = true,
                        Script = MessageBox.Show("عملیات حذف با موفقیت انجام شد", MessageType.Success).Script,
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
    }
}