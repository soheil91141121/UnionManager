using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using UnionManager.Models.DomainModels;
using UnionManager.Models.ExcelModels;
using UnionManager.Models.Repositories;
using UnionManager.ViewModels.Trade;

namespace UnionManager.Controllers
{
    [Authorize(Roles = "Admin, HoleBoss")]
    public class TradeController : Controller
    {
        UnionManager.Helpers.Utilities.SessionHelper sh = new Helpers.Utilities.SessionHelper();

        TradeRepository blTrade = new TradeRepository();
        TradeGroupRepository blTradeGroup = new TradeGroupRepository();
        RelationRepository blRelation = new RelationRepository();

        [HttpGet]
        public ActionResult Trades(int page = 1)
        {
            try
            {
                if (Convert.ToBoolean(Session["isTrade"]) == false)
                {
                    Session["Trade_Name"] = null;
                    Session["Trade_GroupId"] = null;
                    Session["Trade_Tel"] = null;
                    Session["Trade_Address"] = null;
                    Session["Trade_Status"] = null;
                }

                long totalModelCount = 0;
                var trades = Search(ref totalModelCount,
                                      (Session["Trade_Name"] == null) ? null : Session["Trade_Name"].ToString(),
                                      (Session["Trade_GroupId"] == null) ? 0 : Convert.ToInt64(Session["Trade_GroupId"]),
                                      (Session["Trade_Tel"] == null) ? null : Session["Trade_Tel"].ToString(),
                                      (Session["Trade_Address"] == null) ? null : Session["Trade_Address"].ToString(),
                                      (Session["Trade_Status"] == null) ? -1 : Convert.ToInt32(Session["Trade_Status"]));

                int totalItemCount = trades.Count();
                float totalPageCount = (float)Math.Ceiling((float)totalItemCount / 10);
                var model = new TradeViewModel()
                {
                    Trades = trades.OrderBy(p => p.Id).Skip((page - 1) * 10).Take(10).ToList(),
                    TradeGroups = blTradeGroup.Select().ToList(),
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
        public ActionResult AddTrade()
        {
            var model = new AddTradeViewModel()
            {
                TradeGroups = blTradeGroup.Select().ToList()
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult AddTrade(Trade trade)
        {
            try
            {
                //////// validation
                if (!Utilities.IsRequiredStringInputsValid(trade.Name, trade.Tel, trade.Address))
                {
                    return RedirectToAction("Error", "Home", new { message = "فیلدهای ستاره دار را بدرستی وارد کنید" });
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
                        return RedirectToAction("Trades", "Trade");
                    }
                    else
                    {
                        if (message.Contains("Trades(Name And TradeGroupId Must Unique)"))
                        {
                            return RedirectToAction("Error", "Home", new { message = "نام صنف و گروه صنف باید یکتا باشد" });
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

        [HttpGet]
        public ActionResult EditTrade(long? id)
        {
            if (id == null) return RedirectToAction("Error404", "Home");

            var model = new AddTradeViewModel()
            {
                TradeGroups = blTradeGroup.Select().ToList(),
                Trade = blTrade.Find(Convert.ToInt64(id))
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult EditTrade(Trade trade)
        {
            try
            {
                //////// validation
                if (!Utilities.IsRequiredStringInputsValid(trade.Name, trade.Tel, trade.Address))
                {
                    return RedirectToAction("Error", "Home", new { message = "فیلدهای ستاره دار را بدرستی وارد کنید" });
                }

                trade.Status = "فعال";
                trade.Name = trade.Name.ToFarsiString().Trim();
                trade.Tel = trade.Tel.ToFarsiString().Trim();
                trade.Address = trade.Address.ToFarsiString().Trim();
                //////////////

                if (ModelState.IsValid)
                {
                    string message = "";
                    if (blTrade.Update(trade, out message))
                    {
                        return RedirectToAction("Trades", "Trade");
                    }
                    else
                    {
                        if (message.Contains("Trades(Name And TradeGroupId Must Unique)"))
                        {
                            return RedirectToAction("Error", "Home", new { message = "نام صنف و گروه صنف باید یکتا باشد" });
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
        public ActionResult DeleteTrade(long? id)
        {
            try
            {
                if (id == null) return RedirectToAction("Error404", "Home");

                string message = "";
                if (blTrade.Delete(Convert.ToInt64(id), out message))
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
                    if (message.Contains("FK_Trades_TradeGroups") || message.Contains("FK_UserTrade_Trades") || message.Contains("FK_VehicleTrade_Trades"))
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

        //-------------------------------- search -------------------------------------
        [HttpGet]
        public ActionResult SearchTrade(string Name, long? GroupId, string Tel, string Address, int? Status)
        {
            Session["Trade_Name"] = Name;
            Session["Trade_GroupId"] = GroupId;
            Session["Trade_Tel"] = Tel;
            Session["Trade_Address"] = Address;
            Session["Trade_Status"] = Status;
            Session["isTrade"] = true;

            return RedirectToAction("Trades", "Trade");
        }

        public List<Trade> Search(ref long totalModelCount, string Name, long? GroupId, string Tel, string Address, int? Status)
        {
            var trades = blTrade.Select().ToList();
            totalModelCount = trades.Count();

            if (!string.IsNullOrEmpty(Name))
            {
                trades = trades.Where(p => p.Name.Contains(Name.ToFarsiString().Trim())).ToList();
            }
            if (GroupId != null && GroupId > 0)
            {
                trades = trades.Where(p => p.TradeGroupId == GroupId).ToList();
            }
            if (!string.IsNullOrEmpty(Tel))
            {
                trades = trades.Where(p => p.Tel.Contains(Tel.ToFarsiString().Trim())).ToList();
            }
            if (!string.IsNullOrEmpty(Address))
            {
                trades = trades.Where(p => p.Address.Contains(Address.ToFarsiString().Trim())).ToList();
            }
            if (Status != null && Status >= 0)
            {
                if (Status == 1)
                    trades = trades.Where(p => p.Status == "فعال").ToList();
                else
                    trades = trades.Where(p => p.Status == "غیرفعال").ToList();
            }

            return trades;
        }

        //-------------------------------- excel----------------------------------
        private List<TradeExcelModel> GetExcelModel()
        {
            long totalModelCount = 0;
            var trades = Search(ref totalModelCount,
                                  (Session["Trade_Name"] == null) ? null : Session["Trade_Name"].ToString(),
                                  (Session["Trade_GroupId"] == null) ? 0 : Convert.ToInt64(Session["Trade_GroupId"]),
                                  (Session["Trade_Tel"] == null) ? null : Session["Trade_Tel"].ToString(),
                                  (Session["Trade_Address"] == null) ? null : Session["Trade_Address"].ToString(),
                                  (Session["Trade_Status"] == null) ? -1 : Convert.ToInt32(Session["Trade_Status"]));

            var model = new List<TradeExcelModel>();
            TradeExcelModel excelModel;

            foreach (var item in trades)
            {
                excelModel = new TradeExcelModel();
                excelModel.Name = item.Name;
                excelModel.GroupName = item.TradeGroup.Name;
                excelModel.Tel = item.Tel;
                excelModel.Address = item.Address;
                excelModel.Status = item.Status;

                var users = blRelation.Where(p => p.TradeId == item.Id).ToList();
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

                var vehicles = blRelation.Where(p => p.TradeId == item.Id).ToList();
                if (vehicles.Count() != 0)
                {
                    int counter = 0;
                    foreach (var v in vehicles)
                    {
                        if (v.VehicleId != null)
                        {
                            if (counter == 0)
                                excelModel.VehiclesName = v.Vehicle.Model;
                            else
                                excelModel.VehiclesName += " / " + v.Vehicle.Model;
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
                return RedirectToAction("Trades", "Trade");
            }

            output += @"<html dir='rtl'><head><meta http-equiv=Content-Type content=""text/html; charset=utf-8""><style> .text { mso-number-format:\@; } .trFormat{color:Black;background-color:#DEDFDE;} </style></head><table dir=""rtl"" cellspacing=""1"" cellpadding=""3"" border=""0"" style=""background-color:White;border-color:White;border-width:2px;border-style:Ridge;font-family:Tahoma;font-size:11px;width:100%;""><tbody>";
            output += @"<tr><th class='text-center'>نام صنف</th><th class='text-center'>نام گروه</th><th class='text-center'>شماره تلفن</th><th class='text-center'>آدرس</th><th class='text-center'>وضعیت</th><th class='text-center'>کاربران</th><th class='text-center'>وسایل نقلیه</th></tr>";

            foreach (var item in model)
            {
                output += @"<tr><td align='center'>" + item.Name + "</td><td align='center'>" + item.GroupName + "</td><td align='center'>" + item.Tel + "</td><td align='center'>" + item.Address + "</td><td align='center'>" + item.Status + "</td><td align='center'>" + item.UsersName + "</td><td align='center'>" + item.VehiclesName + "</td></tr>";
            }

            output += "</tbody></table></html>";
            Response.Clear();
            Response.ContentEncoding = Encoding.UTF8;
            Response.ContentType = @"application/vnd.xls;";
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}.xls", "Trades_" + DateTime.Now.ToPersianDateString()));
            Response.Write(output);
            Response.End();

            return RedirectToAction("Trades", "Trade");
        }
    }
}