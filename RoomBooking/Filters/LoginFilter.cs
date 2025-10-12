using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RoomBooking.Filters
{
    public class LoginFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            if (controller == "Account")
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            // Check Session
            if (filterContext.HttpContext.Session["UserId"] == null)
            {
                // If no Session, try cookie
                var authCookie = filterContext.HttpContext.Request.Cookies["AuthCookie"];
                if (authCookie != null)
                {
                    filterContext.HttpContext.Session["UserId"] = authCookie.Values["UserId"];
                    filterContext.HttpContext.Session["Username"] = authCookie.Values["Username"];
                }
            }

            // If still no login → redirect
            if (filterContext.HttpContext.Session["UserId"] == null)
            {
                filterContext.Result = new RedirectResult("~/Account/Login");
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}

