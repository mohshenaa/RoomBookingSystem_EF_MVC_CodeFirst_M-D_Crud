using PagedList;
using RoomBooking.Models;
using RoomBooking.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace RoomBooking.Controllers
{
    public class BookingController : Controller
    {
        private HotelDBEntities db = new HotelDBEntities();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["UserId"] == null) 
            {
                filterContext.Result = RedirectToAction("Login", "Account");
                return;
            }
            base.OnActionExecuting(filterContext);
        }

        // GET: Booking
        public ActionResult Index(string sortOrder,string searchString,string currentFilter,int? page)
        {
            ViewBag.currentSort=sortOrder;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.currentFilter=searchString;

                var Bookings = from b in db.BookingMasters select b;
            if (!String.IsNullOrEmpty(searchString))
            {
                Bookings = Bookings.Where(b =>
                b.GuestName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    Bookings= Bookings.OrderByDescending(b=>b.GuestName);
                    break;
                default:
                    Bookings = Bookings.OrderBy(s => s.GuestName);
                    break;
            }
            int pageSize = 4;
            int pageNumber = (page ?? 1);

            return View(Bookings.ToPagedList(pageNumber,pageSize));
        }

        // GET: Booking/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BookingMaster bmaster = db.BookingMasters
                .Include(b => b.BookingDetails.Select(d => d.Room))
                .FirstOrDefault(b => b.BookingId == id);

            if (bmaster == null)
            {
                return HttpNotFound();
            }
            return View(bmaster);
        }

        // GET: Booking/Create
        // GET: Booking/Create
        public ActionResult Create()
        {
            CreateDropdowns();
            var vm = new BookingVM
            {
                BookingDate = DateTime.Today,
                BookingDetails = new List<BookingDetail>() // This list is now empty by default
            };
            return View(vm);
        }

        // POST: Booking/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookingVM booking)
        {
            CreateDropdowns(); // Load dropdowns again if needed

            if (booking.operation == "add")
            {
                booking.BookingDetails.Add(new BookingDetail());
                ModelState.Clear();
                return PartialView("_Data", booking);
            }
            else if (booking.operation.StartsWith("delete"))
            {
                if (int.TryParse(booking.operation.Replace("delete-", ""), out int index))
                {
                    booking.BookingDetails.RemoveAt(index);
                    ModelState.Clear();
                    return PartialView("_Data", booking);
                }
            }

            if (booking.GuestImageFile != null && booking.GuestImageFile.ContentLength > 0)
            {
                var uploadPath = Server.MapPath("~/Content/Images/Guests");
                Directory.CreateDirectory(uploadPath);

                var fileName = Path.GetFileNameWithoutExtension(booking.GuestImageFile.FileName);
                var extension = Path.GetExtension(booking.GuestImageFile.FileName);
                var uniqueFileName = fileName + "_" + Guid.NewGuid().ToString("N").Substring(0, 8) + extension;

                var fullPath = Path.Combine(uploadPath, uniqueFileName);
                booking.GuestImageFile.SaveAs(fullPath);

                // Save the relative web path for the database
                booking.ImagePath = "/Content/Images/Guests/" + uniqueFileName;
            }

            if (ModelState.IsValid)
            {
                var data = booking.Convert();
                db.BookingMasters.Add(data);
                db.SaveChanges();

                foreach (var detail in booking.BookingDetails)
                {
                    var bookedRoom = db.Rooms.Find(detail.RoomId);
                    if (bookedRoom != null)
                    {
                        bookedRoom.Status = true;
                    }
                }

                db.SaveChanges();

                if (Request.IsAjaxRequest())
                {
                    return Content("<script>window.location = '/Booking/Index';</script>");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView(booking);
            }
            else
            {
                return View(booking);
            }
        }


        private void CreateDropdowns()
        {

            ViewBag.Rooms = new SelectList(db.Rooms, "RoomId", "RoomName");
           
        }
        public JsonResult GetRoomPrice(int id)
        {
            var room = db.Rooms.Find(id);
            if (room != null)
            {
                return Json(new { room.PricePerNight }, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        // GET: Booking/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookingMaster bookingMaster = db.BookingMasters.Find(id);
            if (bookingMaster == null)
            {
                return HttpNotFound();
            }
            CreateDropdowns();
            var vm = new BookingVM(bookingMaster);
            return View(vm);
        }

        // POST: Booking/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BookingVM booking)
        {
            CreateDropdowns();

            // Handle "add" and "delete" operations for the partial view
            if (booking.operation == "add")
            {
                booking.BookingDetails.Add(new BookingDetail());
                ModelState.Clear();
                return PartialView("_Data", booking);
            }
            else if (booking.operation.StartsWith("delete"))
            {
                if (int.TryParse(booking.operation.Replace("delete-", ""), out int index))
                {
                    booking.BookingDetails.RemoveAt(index);
                    ModelState.Clear();
                    return PartialView("_Data", booking);
                }
            }
            if (ModelState.IsValid)
            {             
                booking.Bill = (decimal)booking.BookingDetails.Sum(a => a.StayingDays * a.PricePerNight);

                var oldData = db.BookingMasters.Find(booking.BookingId);

                if (oldData != null)
                {
                    db.BookingDetails.RemoveRange(oldData.BookingDetails);
                    db.BookingMasters.Remove(oldData);
                }
                db.BookingMasters.Add(booking.Convert());
                db.SaveChanges();

                // Check if the request is an AJAX request to determine the return type
                if (Request.IsAjaxRequest())
                {
                    return Content("<script>window.location = '/Booking/Index';</script>");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Data", booking);
            }
            else
            {
                return View(booking);
            }
        }

        // GET: Booking/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookingMaster bookingMaster = db.BookingMasters.Find(id);
            if (bookingMaster == null)
            {
                return HttpNotFound();
            }
            return View(bookingMaster);
        }

        // POST: Booking/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BookingMaster bookingMaster = db.BookingMasters.Find(id);
            db.BookingDetails.RemoveRange(bookingMaster.BookingDetails);
            db.BookingMasters.Remove(bookingMaster);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
       

    }
}
