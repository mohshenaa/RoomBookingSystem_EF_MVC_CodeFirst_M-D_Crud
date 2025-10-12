using RoomBooking.Models;
using RoomBooking.ViewModel;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Booking.Controllers
{
    public class AccountController : Controller
    {
        private HotelDBEntities db = new HotelDBEntities();

        // Register
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        //public JsonResult CheckCode(int CourseID)
        //{
        //    return Json(!CheckCourseId(CourseID), JsonRequestBehavior.AllowGet);
        //}


        //private bool CheckCourseId(int id)
        //{
        //    return db.Courses.Any(c => c.CourseID == id);

        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                var exists = db.Users.Any(u => u.Username == model.Username);
                if (exists)
                {
                    ModelState.AddModelError("", "Username already exists");
                    return View(model);
                }

                var user = new User
                {
                    Username = model.Username,
                    Password = model.Password  
                };

                db.Users.Add(user);
                db.SaveChanges();

                return RedirectToAction("Login");
            }
            return View(model);
        }

        // Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.FirstOrDefault(
                    u => u.Username == model.Username && u.Password == model.Password
                );

                if (user != null)
                {
                    Session["UserId"] = user.UserId;
                    Session["Username"] = user.Username;
                    if (model.RememberMe)
                    {
                        HttpCookie authCookie = new HttpCookie("AuthCookie");
                        authCookie.Values["UserId"] = user.UserId.ToString();
                        authCookie.Values["Username"] = user.Username;
                        authCookie.Expires = DateTime.Now.AddDays(60);
                        Response.Cookies.Add(authCookie);

                        FormsAuthentication.SetAuthCookie(user.Username, false); ;
                    }
                    return RedirectToAction("Index", "Booking");
                }

                ModelState.AddModelError("", "Invalid username or password");
            }
            return View(model);
        }

        //Logout
        public ActionResult Logout()
        {
            Session.Clear();

            // remove cookie
            if (Request.Cookies["AuthCookie"] != null)
            {
                var cookie = new HttpCookie("AuthCookie");
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }

            return RedirectToAction("Login");
        }

    }
}
