using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UnionManager.Helpers.Utilities
{
    public class SessionHelper
    {
        public SessionHelper()
        {
            string url = HttpContext.Current.Request.Url.AbsolutePath;
            if (!string.IsNullOrEmpty(url))
            {
                if (url.Contains("/Trade/"))
                {
                    if (HttpContext.Current.Session["isTrade"] == null)
                        HttpContext.Current.Session["isTrade"] = false;
                    else if (Convert.ToBoolean(HttpContext.Current.Session["isTrade"]) == false)
                        HttpContext.Current.Session["isTrade"] = false;
                    else
                        HttpContext.Current.Session["isTrade"] = true;

                    HttpContext.Current.Session["isVehicle"] = false;
                    HttpContext.Current.Session["isUser"] = false;
                }
                else if (url.Contains("/Vehicle/"))
                {
                    if (HttpContext.Current.Session["isVehicle"] == null)
                        HttpContext.Current.Session["isVehicle"] = false;
                    else if (Convert.ToBoolean(HttpContext.Current.Session["isVehicle"]) == false)
                        HttpContext.Current.Session["isVehicle"] = false;
                    else
                        HttpContext.Current.Session["isVehicle"] = true;

                    HttpContext.Current.Session["isTrade"] = false;
                    HttpContext.Current.Session["isUser"] = false;
                }
                else if (url.Contains("/User/"))
                {
                    if (HttpContext.Current.Session["isUser"] == null)
                        HttpContext.Current.Session["isUser"] = false;
                    else if (Convert.ToBoolean(HttpContext.Current.Session["isUser"]) == false)
                        HttpContext.Current.Session["isUser"] = false;
                    else
                        HttpContext.Current.Session["isUser"] = true;

                    HttpContext.Current.Session["isTrade"] = false;
                    HttpContext.Current.Session["isVehicle"] = false;
                }
                else
                {
                    HttpContext.Current.Session["isTrade"] = false;
                    HttpContext.Current.Session["isVehicle"] = false;
                    HttpContext.Current.Session["isUser"] = false;
                }
            }
        }
    }
}