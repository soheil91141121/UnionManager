using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UnionManager.Models.DomainModels;
using UnionManager.Models.Repositories;

namespace UnionManager.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        UnionManager.Helpers.Utilities.SessionHelper sh = new Helpers.Utilities.SessionHelper();

        UserRepository blUser = new UserRepository();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Error404()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Error(string message)
        {
            ViewBag.Message = message;
            return View();
        }

        public string GetUserImage()
        {
            string image = blUser.Where(p => p.Username == User.Identity.Name).Single().Image;
            if (string.IsNullOrEmpty(image))
                return "";
            else
                return image;
        }
    }
}