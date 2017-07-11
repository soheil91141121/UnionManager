using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UnionManager.Models.DomainModels;
using UnionManager.Models.Repositories;
using UnionManager.ViewModels.Relation;

namespace UnionManager.Controllers
{
    [Authorize(Roles = "Admin, HoleBoss")]
    public class RelationController : Controller
    {
        UnionManager.Helpers.Utilities.SessionHelper sh = new Helpers.Utilities.SessionHelper();

        RelationRepository blRelation = new RelationRepository();
        TradeRepository blTrade = new TradeRepository();
        TradeGroupRepository blTradeGroup = new TradeGroupRepository();
        VehicleRepository blVehicle = new VehicleRepository();
        VehicleGroupRepository blVehicleGroup = new VehicleGroupRepository();
        ColorRepository blColor = new ColorRepository();
        UserRepository blUser = new UserRepository();
        RoleRepository blRole = new RoleRepository();

        [HttpGet]
        public ActionResult Relation(long? tradeId)
        {
            try
            {
                var model = new RelationViewModel()
                {
                    Relations = new List<Models.DomainModels.Relation>(),
                    Trades = blTrade.Where(p => p.Status == "فعال").ToList(),
                    TradeGroups = blTradeGroup.Where(p => p.Status == "فعال").ToList(),
                    Users = blUser.Where(p => p.Status == "فعال" && p.Role.RoleName != "Admin" && p.Role.RoleName != "HoleBoss").ToList(),
                    Roles = blRole.Where(p => p.Status == "فعال" && p.RoleName != "Admin" && p.RoleName != "HoleBoss").ToList(),
                    Vehicles = blVehicle.Where(p => p.Status == "فعال").ToList(),
                    VehicleGroups = blVehicleGroup.Where(p => p.Status == "فعال").ToList(),
                    Colors = blColor.Select().ToList(),
                    User = new User { Gender = true },
                };

                if (tradeId != null)
                {
                    model.Trade = blTrade.Where(p => p.Id == tradeId).Single();
                    model.Relations = blRelation.Where(p => p.TradeId == tradeId).ToList();
                }

                return View(model);
            }
            catch
            {
                return RedirectToAction("Error404", "Home");
            }
        }

        //---------------- trade ------------------
        [HttpPost]
        public JsonResult AddTradeRelation(Trade trade)
        {
            try
            {
                //////// validation
                if (!Utilities.IsRequiredStringInputsValid(trade.Name, trade.Tel, trade.Address))
                {
                    return Json(new JsonData()
                    {
                        Success = false,
                        Script = MessageBox.Show("فیلدهای ستاره دار را بدرستی وارد کنید", MessageType.Error).Script,
                        Html = ""
                    });
                }

                trade.Status = "فعال";
                trade.Name = trade.Name.ToFarsiString().Trim();
                trade.Tel = trade.Tel.ToFarsiString().Trim();
                trade.Address = trade.Address.ToFarsiString().Trim();
                //////////////

                if (ModelState.IsValid)
                {
                    string message = "";
                    if (blTrade.Add(trade, out message))
                    {
                        return Json(new
                        {
                            Success = true,
                            Script = MessageBox.Show("عملیات درج با موفقیت انجام شد", MessageType.Success).Script,
                            Html = "",
                            Id = blTrade.Where(p => p.Name == trade.Name && p.TradeGroupId == trade.TradeGroupId).Single().Id
                        });
                    }
                    else
                    {
                        if (message.Contains("Trades(Name And TradeGroupId Must Unique)"))
                        {
                            return Json(new JsonData()
                            {
                                Success = false,
                                Script = MessageBox.Show("نام صنف و گروه صنف باید یکتا باشد", MessageType.Error).Script,
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
        public JsonResult SearchTradeRelation(string name, int? groupId)
        {
            var model = new RelationViewModel();
            model.TradeGroups = blTradeGroup.Select().ToList();

            if (!string.IsNullOrEmpty(name.Trim()) && groupId != null)
            {
                string text = name.ToFarsiString().Trim();
                model.Trades = blTrade.Where(p => p.Name.Contains(text) && p.TradeGroupId == groupId && p.Status == "فعال").ToList();
            }
            else if (string.IsNullOrEmpty(name.Trim()) && groupId != null)
                model.Trades = blTrade.Where(p => p.TradeGroupId == groupId && p.Status == "فعال").ToList();
            else if (!string.IsNullOrEmpty(name.Trim()) && groupId == null)
            {
                string text = name.ToFarsiString().Trim();
                model.Trades = blTrade.Where(p => p.Name.Contains(text) && p.Status == "فعال").ToList();
            }
            else
                model.Trades = blTrade.Where(p => p.Status == "فعال").ToList();

            return Json(new JsonData()
            {
                Success = true,
                Script = "",
                Html = this.RenderPartialToString("_TradeListRelation", model)
            });
        }

        [HttpPost]
        public JsonResult GetTradeById(int id)
        {
            var model = blTrade.Where(p => p.Id == id).Single();

            return Json(new { id = model.Id, name = model.Name, groupName = model.TradeGroup.Name, tel = model.Tel, address = model.Address });
        }

        //---------------- user ------------------
        [HttpPost]
        public JsonResult AddUserRelation(User user)
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
        public JsonResult SearchUserRelation(string name, int? roleId)
        {
            var model = new RelationViewModel();
            model.Roles = blRole.Where(p => p.Status == "فعال" && p.RoleName != "Admin" && p.RoleName != "HoleBoss").ToList();
            var users = blUser.Where(p => p.Status == "فعال" && p.Role.RoleName != "Admin" && p.Role.RoleName != "HoleBoss").ToList();

            if (!string.IsNullOrEmpty(name))
                users = users.Where(p => p.Name.Contains(name.ToFarsiString().Trim())).ToList();
            if (roleId != null)
                users = users.Where(p => p.RoleId == roleId).ToList();

            model.Users = users;

            return Json(new JsonData()
            {
                Success = true,
                Script = "",
                Html = this.RenderPartialToString("_UserListRelation", model)
            });
        }

        [HttpPost]
        public ActionResult AddRelationForUser(long userId, long tradeId)
        {
            try
            {
                User user = blUser.Find(userId);
                user.ConfirmPassword = user.Password;

                var roleName = blUser.Find(userId).Role.RoleName;
                if (roleName == "Boss")
                {
                    bool isThereBoss = blRelation.Where(p => p.TradeId == tradeId).Where(x => x.User.Role.RoleName == "Boss").Any();
                    if (isThereBoss)
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

                        return Json(new JsonData()
                        {
                            Success = false,
                            Script = MessageBox.Show("هر صنف حداکثر دارای یک مدیر است", MessageType.Error).Script,
                            Html = ""
                        });
                    }
                }

                var relation = new Relation() { UserId = userId, TradeId = tradeId, Status = "تمام وقت" };
                string message = "";
                if (blRelation.Add(relation, out message))
                {
                    Session["User_Id"] = userId;
                    Session["Relation_Id"] = blRelation.Where(p => p.UserId == userId && p.TradeId == tradeId).Single().Id;
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
                //for (int i = 0; i < Request.Files.Count; i++) { }
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
                    long userId = Convert.ToInt64(Session["User_Id"]);
                    long relationId = Convert.ToInt64(Session["Relation_Id"]);

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
                blUser.Delete(Convert.ToInt64(Session["User_Id"]), out message);
                string message2 = "";
                blRelation.Delete(Convert.ToInt64(Session["Relation_Id"]), out message2);

                return Json(new JsonData()
                {
                    Success = false,
                    Script = MessageBox.Show("در آپلود تصویر مشکل پیش آمده است. مجددا تلاش کنید", MessageType.Error).Script,
                    Html = ""
                });
            }

        }

        [HttpPost]
        public JsonResult AddRelationForUserById(long userId, long tradeId)
        {
            try
            {
                var roleName = blUser.Find(userId).Role.RoleName;
                if (roleName == "Boss")
                {
                    bool isThereBossInAnyTrades = blRelation.Where(p => p.UserId == userId && p.TradeId != tradeId).Any();
                    bool isThereBoss = blRelation.Where(p => p.TradeId == tradeId).Where(x => x.User.Role.RoleName == "Boss").Any();
                    
                    if(isThereBossInAnyTrades)
                    {
                        return Json(new JsonData()
                        {
                            Success = false,
                            Script = MessageBox.Show("این کاربر در صنف دیگری نقش مدیر دارد", MessageType.Error).Script,
                            Html = ""
                        });
                    }
                    if (isThereBoss)
                    {
                        return Json(new JsonData()
                        {
                            Success = false,
                            Script = MessageBox.Show("هر صنف حداکثر دارای یک مدیر است", MessageType.Error).Script,
                            Html = ""
                        });
                    }
                }

                var relation = new Relation() { UserId = userId, TradeId = tradeId, Status = "تمام وقت" };
                string message = "";
                if (blRelation.Add(relation, out message))
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

        //---------------- vehicle ------------------
        [HttpPost]
        public JsonResult AddVehicleRelation(Vehicle vehicle, string np_FirstNum, string np_Letter, string np_SecondNum, string np_ThirdNum)
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
        public JsonResult SearchVehicleRelation(string mdl, int? groupId)
        {
            var model = new RelationViewModel();
            model.VehicleGroups = blVehicleGroup.Where(p => p.Status == "فعال").ToList();
            var vehicles = blVehicle.Where(p => p.Status == "فعال").ToList();

            if (!string.IsNullOrEmpty(mdl))
                vehicles = vehicles.Where(p => p.Model.Contains(mdl.ToFarsiString().Trim())).ToList();
            if (groupId != null)
                vehicles = vehicles.Where(p => p.VehicleGroupId == groupId).ToList();

            model.Vehicles = vehicles;

            return Json(new JsonData()
            {
                Success = true,
                Script = "",
                Html = this.RenderPartialToString("_VehicleListRelation", model)
            });
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
        public JsonResult AddRelationForVehicleById(long vehicleId, long relationId)
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