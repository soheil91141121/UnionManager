using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UnionManager.Models.DomainModels;
using UnionManager.Models.Repositories;
using UnionManager.ViewModels.GroupAndColor;

namespace UnionManager.Controllers
{
    [Authorize(Roles = "Admin, HoleBoss")]
    public class GroupAndColorController : Controller
    {
        UnionManager.Helpers.Utilities.SessionHelper sh = new Helpers.Utilities.SessionHelper();

        TradeGroupRepository blTradeGroup = new TradeGroupRepository();
        VehicleGroupRepository blVehicleGroup = new VehicleGroupRepository();
        ColorRepository blColor = new ColorRepository();

        [HttpGet]
        public ActionResult Groups()
        {
            var model = new GroupViewModel()
            {
                TradeGroups = blTradeGroup.Select().ToList(),
                VehicleGroups = blVehicleGroup.Select().ToList(),
                Colors = blColor.Select().ToList()
            };

            return View(model);
        }

        //----------------------- tradeGroup -------------------------
        [HttpGet]
        public ActionResult AddTradeGroup()
        {
            var model = new TradeGroup();
            return View(model);
        }

        [HttpPost]
        public ActionResult AddTradeGroup(TradeGroup tradeGroup)
        {
            try
            {
                //////// validation
                if (!Utilities.IsRequiredStringInputsValid(tradeGroup.Name))
                {
                    return RedirectToAction("Error", "Home", new { message = "فیلدهای ستاره دار را بدرستی وارد کنید" });
                }

                tradeGroup.Status = "فعال";
                tradeGroup.Name = tradeGroup.Name.ToFarsiString().Trim();
                //////////////

                if (ModelState.IsValid)
                {
                    string message = "";
                    if (blTradeGroup.Add(tradeGroup, out message))
                    {
                        return RedirectToAction("Groups", "GroupAndColor");
                    }
                    else
                    {
                        if (message.Contains("TradeGroups(Name Must Unique)"))
                        {
                            return RedirectToAction("Error", "Home", new { message = "نام گروه صنف باید یکتا باشد" });
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
        public ActionResult EditTradeGroup(int id)
        {
            var model = blTradeGroup.Find(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult EditTradeGroup(TradeGroup tradeGroup)
        {
            try
            {
                //////// validation
                if (!Utilities.IsRequiredStringInputsValid(tradeGroup.Name))
                {
                    return RedirectToAction("Error", "Home", new { message = "فیلدهای ستاره دار را بدرستی وارد کنید" });
                }

                tradeGroup.Status = "فعال";
                tradeGroup.Name = tradeGroup.Name.ToFarsiString().Trim();
                //////////////

                if (ModelState.IsValid)
                {
                    string message = "";
                    if (blTradeGroup.Update(tradeGroup, out message))
                    {
                        return RedirectToAction("Groups", "GroupAndColor");
                    }
                    else
                    {
                        if (message.Contains("TradeGroups(Name Must Unique)"))
                        {
                            return RedirectToAction("Error", "Home", new { message = "نام گروه صنف باید یکتا باشد" });
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
        public ActionResult DeleteTradeGroup(int id)
        {
            try
            {
                string message = "";
                if (blTradeGroup.Delete(id, out message))
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
                    if (message.Contains("FK_Trades_TradeGroups"))
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

        //---------------------------- VehicleGroup ----------------------------
        [HttpGet]
        public ActionResult AddVehicleGroup()
        {
            var model = new VehicleGroup();
            return View(model);
        }

        [HttpPost]
        public ActionResult AddVehicleGroup(VehicleGroup vehicleGroup)
        {
            try
            {
                //////// validation
                if (!Utilities.IsRequiredStringInputsValid(vehicleGroup.Name))
                {
                    return RedirectToAction("Error", "Home", new { message = "فیلدهای ستاره دار را بدرستی وارد کنید" });
                }

                vehicleGroup.Status = "فعال";
                vehicleGroup.Name = vehicleGroup.Name.ToFarsiString().Trim();
                //////////////

                if (ModelState.IsValid)
                {
                    string message = "";
                    if (blVehicleGroup.Add(vehicleGroup, out message))
                    {
                        return RedirectToAction("Groups", "GroupAndColor");
                    }
                    else
                    {
                        if (message.Contains("VehicleGroups(Name Must Unique)"))
                        {
                            return RedirectToAction("Error", "Home", new { message = "نام گروه وسیله نقلیه باید یکتا باشد" });
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
        public ActionResult EditVehicleGroup(int id)
        {
            var model = blVehicleGroup.Find(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult EditVehicleGroup(VehicleGroup vehicleGroup)
        {
            try
            {
                //////// validation
                if (!Utilities.IsRequiredStringInputsValid(vehicleGroup.Name))
                {
                    return RedirectToAction("Error", "Home", new { message = "فیلدهای ستاره دار را بدرستی وارد کنید" });
                }

                vehicleGroup.Status = "فعال";
                vehicleGroup.Name = vehicleGroup.Name.ToFarsiString().Trim();
                //////////////

                if (ModelState.IsValid)
                {
                    string message = "";
                    if (blVehicleGroup.Update(vehicleGroup, out message))
                    {
                        return RedirectToAction("Groups", "GroupAndColor");
                    }
                    else
                    {
                        if (message.Contains("VehicleGroups(Name Must Unique)"))
                        {
                            return RedirectToAction("Error", "Home", new { message = "نام گروه وسیله نقلیه باید یکتا باشد" });
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
        public ActionResult DeleteVehicleGroup(int id)
        {
            try
            {
                string message = "";
                if (blVehicleGroup.Delete(id, out message))
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
                    if (message.Contains("FK_Vehicles_VehicleGroups"))
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

        //----------------------- color -------------------------
        [HttpGet]
        public ActionResult AddColor()
        {
            var model = new Color();
            return View(model);
        }

        [HttpPost]
        public ActionResult AddColor(Color color)
        {
            try
            {
                //////// validation
                if (!Utilities.IsRequiredStringInputsValid(color.Name))
                {
                    return RedirectToAction("Error", "Home", new { message = "فیلدهای ستاره دار را بدرستی وارد کنید" });
                }

                color.Status = "فعال";
                color.Name = color.Name.ToFarsiString().Trim();
                //////////////

                if (ModelState.IsValid)
                {
                    string message = "";
                    if (blColor.Add(color, out message))
                    {
                        return RedirectToAction("Groups", "GroupAndColor");
                    }
                    else
                    {
                        if (message.Contains("Colors(Name Must Unique)"))
                        {
                            return RedirectToAction("Error", "Home", new { message = "نام رنگ باید یکتا باشد" });
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
        public ActionResult EditColor(int id)
        {
            var model = blColor.Find(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult EditColor(Color color)
        {
            try
            {
                //////// validation
                if (!Utilities.IsRequiredStringInputsValid(color.Name))
                {
                    return RedirectToAction("Error", "Home", new { message = "فیلدهای ستاره دار را بدرستی وارد کنید" });
                }

                color.Status = "فعال";
                color.Name = color.Name.ToFarsiString().Trim();
                //////////////

                if (ModelState.IsValid)
                {
                    string message = "";
                    if (blColor.Update(color, out message))
                    {
                        return RedirectToAction("Groups", "GroupAndColor");
                    }
                    else
                    {
                        if (message.Contains("Colors(Name Must Unique)"))
                        {
                            return RedirectToAction("Error", "Home", new { message = "نام رنگ باید یکتا باشد" });
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
        public ActionResult DeleteColor(int id)
        {
            try
            {
                string message = "";
                if (blColor.Delete(id, out message))
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
                    if (message.Contains("FK_Vehicles_Colors"))
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
    }
}