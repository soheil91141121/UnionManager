using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UnionManager.Models.DomainModels;
using UnionManager.Models.Repositories;
using UnionManager.ViewModels.Vehicle;
using System.Linq.Expressions;
using System.Text;
using UnionManager.Models.ExcelModels;

namespace UnionManager.Controllers
{
    [Authorize(Roles = "Admin, HoleBoss")]
    public class VehicleController : Controller
    {
        UnionManager.Helpers.Utilities.SessionHelper sh = new Helpers.Utilities.SessionHelper();

        VehicleRepository blVehicle = new VehicleRepository();
        VehicleGroupRepository blVehicleGroup = new VehicleGroupRepository();
        ColorRepository blColor = new ColorRepository();
        RelationRepository blRelation = new RelationRepository();

        [HttpGet]
        public ActionResult Vehicles(int page = 1)
        {
            try
            {
                if (Convert.ToBoolean(Session["isVehicle"]) == false)
                {
                    Session["Vehicle_Model"] = null;
                    Session["Vehicle_GroupId"] = null;
                    Session["Vehicle_NumberPlates"] = null;
                    Session["Vehicle_VINName"] = null;
                    Session["Vehicle_ColorId"] = null;
                    Session["Vehicle_IsHybrid"] = null;
                    Session["Vehicle_Status"] = null;
                }

                long totalModelCount = 0;
                var vehicles = Search(ref totalModelCount,
                                     (Session["Vehicle_Model"] == null) ? null : Session["Vehicle_Model"].ToString(),
                                     (Session["Vehicle_GroupId"] == null) ? 0 : Convert.ToInt64(Session["Vehicle_GroupId"]),
                                     (Session["Vehicle_NumberPlates"] == null) ? null : Session["Vehicle_NumberPlates"].ToString(),
                                     (Session["Vehicle_VINName"] == null) ? null : Session["Vehicle_VINName"].ToString(),
                                     (Session["Vehicle_ColorId"] == null) ? 0 : Convert.ToInt64(Session["Vehicle_ColorId"]),
                                     (Session["Vehicle_IsHybrid"] == null) ? -1 : Convert.ToInt32(Session["Vehicle_IsHybrid"]),
                                     (Session["Vehicle_Status"] == null) ? -1 : Convert.ToInt32(Session["Vehicle_Status"]));

                int totalItemCount = vehicles.Count();
                float totalPageCount = (float)Math.Ceiling((float)totalItemCount / 10);
                var model = new VehicleViewModel()
                {
                    Vehicles = vehicles.OrderBy(p => p.Id).Skip((page - 1) * 10).Take(10).ToList(),
                    VehicleGroups = blVehicleGroup.Select().ToList(),
                    Colors = blColor.Select().ToList(),
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
        public ActionResult AddVehicle()
        {
            var model = new AddVehicleViewModel()
            {
                VehicleGroups = blVehicleGroup.Select().ToList(),
                Colors = blColor.Select().ToList()
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult AddVehicle(Vehicle vehicle, string np_FirstNum, string np_Letter, string np_SecondNum, string np_ThirdNum)
        {
            try
            {
                //////// validation
                if (!Utilities.IsRequiredStringInputsValid(vehicle.Model, vehicle.VINName))
                {
                    return RedirectToAction("Error", "Home", new { message = "فیلدهای ستاره دار را بدرستی وارد کنید" });
                }

                string numberPlates = GetNumberPlates(np_FirstNum, np_Letter, np_SecondNum, np_ThirdNum);
                if (numberPlates == null)
                {
                    return RedirectToAction("Error", "Home", new { message = "شماره پلاک را بدرستی وارد کنید" });
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
                        return RedirectToAction("Vehicles", "Vehicle");
                    }
                    else
                    {
                        if (message.Contains("Vehicles(NumberPlates Must Unique)"))
                        {
                            return RedirectToAction("Error", "Home", new { message = "شماره پلاک باید یکتا باشد" });
                        }

                        if (message.Contains("Vehicles(VINName Must Unique)"))
                        {
                            return RedirectToAction("Error", "Home", new { message = "نام وین باید یکتا باشد" });
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
        public ActionResult EditVehicle(long? id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("Error404", "Home");
                }

                var model = new AddVehicleViewModel()
                {
                    VehicleGroups = blVehicleGroup.Select().ToList(),
                    Colors = blColor.Select().ToList(),
                    Vehicle = blVehicle.Find(Convert.ToInt64(id))
                };

                return View(model);
            }
            catch
            {
                return RedirectToAction("Error404", "Home");
            }
        }

        [HttpPost]
        public ActionResult EditVehicle(Vehicle vehicle, string np_FirstNum, string np_Letter, string np_SecondNum, string np_ThirdNum)
        {
            try
            {
                //////// validation
                if (!Utilities.IsRequiredStringInputsValid(vehicle.Model, vehicle.VINName))
                {
                    return RedirectToAction("Error", "Home", new { message = "فیلدهای ستاره دار را بدرستی وارد کنید" });
                }

                string numberPlates = GetNumberPlates(np_FirstNum, np_Letter, np_SecondNum, np_ThirdNum);
                if (numberPlates == null)
                {
                    return RedirectToAction("Error", "Home", new { message = "شماره پلاک را بدرستی وارد کنید" });
                }

                vehicle.Status = "فعال";
                vehicle.NumberPlates = numberPlates;
                vehicle.Model = vehicle.Model.ToFarsiString().Trim();
                vehicle.VINName = vehicle.VINName.ToFarsiString().Trim();
                //////////////

                if (ModelState.IsValid)
                {
                    string message = "";
                    if (blVehicle.Update(vehicle, out message))
                        return RedirectToAction("Vehicles", "Vehicle");
                    else
                    {
                        if (message.Contains("Vehicles(NumberPlates Must Unique)"))
                        {
                            return RedirectToAction("Error", "Home", new { message = "شماره پلاک باید یکتا باشد" });
                        }

                        if (message.Contains("Vehicles(VINName Must Unique)"))
                        {
                            return RedirectToAction("Error", "Home", new { message = "نام وین باید یکتا باشد" });
                        }

                        return RedirectToAction("Error", "Home", new { message = "خطا رخ داده است. مجددا تلاش کنید" });
                    }
                }
                else
                    return RedirectToAction("Error", "Home", new { message = "خطا رخ داده است. مجددا تلاش کنید" });
            }
            catch
            {
                return RedirectToAction("Error", "Home", new { message = "خطا رخ داده است. مجددا تلاش کنید" });
            }
        }

        [HttpPost]
        public ActionResult DeleteVehicle(long? id)
        {
            try
            {
                if (id == null) return RedirectToAction("Error404", "Home");

                string message = "";
                if (blVehicle.Delete(Convert.ToInt64(id), out message))
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
                    if (message.Contains("FK_UserVehicle_Vehicles") || message.Contains("FK_Vehicles_VehicleGroups") || message.Contains("FK_VehicleTrade_Vehicles"))
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

        //-------------------------------- search -------------------------------------
        [HttpGet]
        public ActionResult SearchVehicle(string Model, long? GroupId, string NumberPlates, string VINName, long? ColorId, int? IsHybrid, int? Status)
        {
            Session["Vehicle_Model"] = Model;
            Session["Vehicle_GroupId"] = GroupId;
            Session["Vehicle_NumberPlates"] = NumberPlates;
            Session["Vehicle_VINName"] = VINName;
            Session["Vehicle_ColorId"] = ColorId;
            Session["Vehicle_IsHybrid"] = IsHybrid;
            Session["Vehicle_Status"] = Status;
            Session["isVehicle"] = true;

            return RedirectToAction("Vehicles", "Vehicle");
        }

        public List<Vehicle> Search(ref long totalModelCount, string Model, long? GroupId, string NumberPlates, string VINName, long? ColorId, int? IsHybrid, int? Status)
        {
            var vehicles = blVehicle.Select().ToList();
            totalModelCount = vehicles.Count();

            if (!string.IsNullOrEmpty(Model))
            {
                vehicles = vehicles.Where(p => p.Model.Contains(Model.ToFarsiString().Trim())).ToList();
            }
            if (GroupId != null && GroupId > 0)
            {
                vehicles = vehicles.Where(p => p.VehicleGroupId == GroupId).ToList();
            }
            if (!string.IsNullOrEmpty(NumberPlates))
            {
                vehicles = vehicles.Where(p => p.NumberPlates.Contains(NumberPlates.ToFarsiString().Trim())).ToList();
            }
            if (!string.IsNullOrEmpty(VINName))
            {
                vehicles = vehicles.Where(p => p.VINName.Contains(VINName.ToFarsiString().Trim())).ToList();
            }
            if (ColorId != null && ColorId > 0)
            {
                vehicles = vehicles.Where(p => p.ColorId == ColorId).ToList();
            }
            if (IsHybrid != null && IsHybrid >= 0)
            {
                if (IsHybrid == 0)
                    vehicles = vehicles.Where(p => p.IsHybrid == false).ToList();
                else
                    vehicles = vehicles.Where(p => p.IsHybrid == true).ToList();
            }
            if (Status != null && Status >= 0)
            {
                if (Status == 1)
                    vehicles = vehicles.Where(p => p.Status == "فعال").ToList();
                else
                    vehicles = vehicles.Where(p => p.Status == "غیرفعال").ToList();
            }

            return vehicles;
        }


        //-------------------------------- detail -------------------------------------
        [HttpGet]
        public ActionResult VehicleDetail(long? id)
        {
            try
            {
                if (id != null)
                {
                    var relation = blRelation.Where(p => p.VehicleId != null).ToList();
                    relation = relation.Where(p => p.VehicleId == Convert.ToInt64(id)).ToList();

                    string users = "", trades = "";

                    int i = 0;
                    foreach (var item in relation)
                    {
                        if (i != 0) users += " /";
                        users += item.User.Name + " " + item.User.Family;
                        i++;
                    }

                    int counter = 0;
                    foreach (var item in relation)
                    {
                        if (counter != 0) trades += " /";
                        trades += item.Trade.Name;
                        counter++;
                    }

                    var model = new VehicleDetailViewModel()
                    {
                        Vehicle = blVehicle.Find(Convert.ToInt64(id)),
                        UserVehicles = (users == "") ? "-" : users,
                        VehicleTrades = (trades == "") ? "-" : trades
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
        private List<VehicleExcelModel> GetExcelModel()
        {
            long totalModelCount = 0;
            var vehicles = Search(ref totalModelCount,
                     (Session["Vehicle_Model"] == null) ? null : Session["Vehicle_Model"].ToString(),
                     (Session["Vehicle_GroupId"] == null) ? 0 : Convert.ToInt64(Session["Vehicle_GroupId"]),
                     (Session["Vehicle_NumberPlates"] == null) ? null : Session["Vehicle_NumberPlates"].ToString(),
                     (Session["Vehicle_VINName"] == null) ? null : Session["Vehicle_VINName"].ToString(),
                     (Session["Vehicle_ColorId"] == null) ? 0 : Convert.ToInt64(Session["Vehicle_ColorId"]),
                     (Session["Vehicle_IsHybrid"] == null) ? -1 : Convert.ToInt32(Session["Vehicle_IsHybrid"]),
                     (Session["Vehicle_Status"] == null) ? -1 : Convert.ToInt32(Session["Vehicle_Status"]));

            var model = new List<VehicleExcelModel>();
            VehicleExcelModel excelModel;

            foreach (var item in vehicles)
            {
                excelModel = new VehicleExcelModel();
                excelModel.Model = item.Model;
                excelModel.GroupName = item.VehicleGroup.Name;
                excelModel.NumberPlates = item.NumberPlates;
                excelModel.VINName = item.VINName;
                excelModel.ColorName = item.Color.Name;
                excelModel.IsHybrid = (item.IsHybrid == true) ? "بله" : "خیر";
                excelModel.Status = item.Status;

                var relation = blRelation.Where(p => p.VehicleId != null).ToList();
                var users = relation.Where(p => p.VehicleId == Convert.ToInt64(item.Id)).ToList();
                if (users.Count() != 0)
                {
                    int counter = 0;
                    foreach (var u in users)
                    {
                        if (counter == 0)
                            excelModel.UsersName = u.User.Name + " " + u.User.Family;
                        else
                            excelModel.UsersName += " / " + u.User.Name + " " + u.User.Family;

                        counter++;
                    }
                }
                else
                {
                    excelModel.UsersName = "-";
                }

                var trades = relation.Where(p => p.VehicleId == Convert.ToInt64(item.Id)).ToList();
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
                return RedirectToAction("Vehicles", "Vehicle");
            }

            output += @"<html dir='rtl'><head><meta http-equiv=Content-Type content=""text/html; charset=utf-8""><style> .text { mso-number-format:\@; } .trFormat{color:Black;background-color:#DEDFDE;} </style></head><table dir=""rtl"" cellspacing=""1"" cellpadding=""3"" border=""0"" style=""background-color:White;border-color:White;border-width:2px;border-style:Ridge;font-family:Tahoma;font-size:11px;width:100%;""><tbody>";
            output += @"<tr><th class='text-center'>مدل</th><th class='text-center'>نام گروه</th><th class='text-center'>شماره پلاک </th><th class='text-center'>نام VIN</th><th class='text-center'>رنگ</th><th class='text-center'>دوگانه سوز</th><th class='text-center'>وضعیت</th><th class='text-center'>کاربران</th><th class='text-center'>اصناف</th></tr>";

            foreach (var item in model)
            {
                output += @"<tr><td align='center'>" + item.Model + "</td><td align='center'>" + item.GroupName + "</td><td align='center'>" + item.NumberPlates + "</td><td align='center'>" + item.VINName + "</td><td align='center'>" + item.ColorName + "</td><td align='center'>" + item.IsHybrid + "</td><td align='center'>" + item.Status + "</td><td align='center'>" + item.UsersName + "</td><td align='center'>" + item.TradesName + "</td></tr>";
            }

            output += "</tbody></table></html>";
            Response.Clear();
            Response.ContentEncoding = Encoding.UTF8;
            Response.ContentType = @"application/vnd.xls;";
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}.xls", "Vehicles_" + DateTime.Now.ToPersianDateString()));
            Response.Write(output);
            Response.End();

            return RedirectToAction("Vehicles", "Vehicle");
        }
    }
}