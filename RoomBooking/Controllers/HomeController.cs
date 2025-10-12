using RoomBooking.Models;
using RoomBooking.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace RoomBooking.Controllers
{
    public class HomeController : Controller
    {
        private HotelDBEntities db = new HotelDBEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            IQueryable<BookingDateGroup> data = from Bookings in db.BookingMasters
                                                group Bookings by Bookings.BookingDate into dateGroup
                                                   select new BookingDateGroup()
                                                   {
                                                       BookingDate = dateGroup.Key,
                                                       BookingCount = dateGroup.Count()
                                                   };
            return View(data.ToList());
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}