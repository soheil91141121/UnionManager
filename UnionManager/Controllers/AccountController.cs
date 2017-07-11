using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UnionManager.Models.Repositories;
using System.Web.Security;
using UnionManager.Models.DomainModels;

namespace UnionManager.Controllers
{
    public class AccountController : Controller
    {
        UnionManager.Helpers.Utilities.SessionHelper sh = new Helpers.Utilities.SessionHelper();

        UserRepository blUser = new UserRepository();

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string Username, string Password, bool RememberMe, string Captcha)
        {
            if (!User.Identity.IsAuthenticated)
            {
                if (Session["CAPTCHA"] != null)
                {
                    if (string.IsNullOrEmpty(Captcha))
                    {
                        ViewBag.Message = "حاصل تصویر امنیتی را وارد کنید";
                        return View();
                    }
                    if (Captcha != Session["CAPTCHA"].ToString().ToLower())
                    {
                        ViewBag.Message = "حاصل تصویر امنیتی را بدرستی وارد کنید";
                        return View();
                    }
                }

                if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
                {
                    ViewBag.Message = "نام کاربری و کلمه عبور را وارد کنید.";
                    return View();
                }

                if (blUser.Exist(Username, Password.Encrypt()))
                {
                    var user = blUser.Where(p => p.Username == Username).Single();
                    if (user.Status != "فعال")
                    {
                        ViewBag.Message = "این کاربر اجازه ورود ندارد";
                        return View();
                    }
                    else if (user.Role.RoleName != "Admin" && user.Role.RoleName != "HoleBoss" && user.Role.RoleName != "Boss")
                    {
                        ViewBag.Message = "این نقش کاربری اجازه ورود ندارد";
                        return View();
                    }
                    else
                    {
                        FormsAuthentication.SetAuthCookie(Username, RememberMe);
                        return Redirect(FormsAuthentication.DefaultUrl);
                    }
                }
                else
                {
                    ViewBag.Message = "نام کاربری یا کلمه عبور اشتباه است.";
                    return View();
                }
            }


            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogOut()
        {
            if (User.Identity.IsAuthenticated)
                FormsAuthentication.SignOut();

            return RedirectToAction("Login", "Account");
        }
    }
}