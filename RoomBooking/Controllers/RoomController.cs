using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualBasic.ApplicationServices;
using RoomBooking.Models;

namespace RoomBooking.Controllers
{
    public class RoomController : Controller
    {
        private HotelDBEntities db = new HotelDBEntities();

        // GET: Room
        public ActionResult Index()
        {
            return View(db.Rooms.ToList());
        }

        // GET: Room/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // GET: Room/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Room/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RoomId,RoomName,ImagePath,PricePerNight,Status")] Room room, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                if (Image != null)
                {
                    if (Image.ContentLength > 1 * 1024 * 1024)
                    {
                        ModelState.AddModelError("Image", $"upload limit is 1MB,upload file size is {Image.ContentLength / (1 * 1024 * 1024)}MB");
                    }
                    string imagepath = $@"\Content\images\{Guid.NewGuid()}-{Image.FileName}";
                    string filepath = Server.MapPath(imagepath);
                    Image.SaveAs(filepath);
                    room.ImagePath = imagepath;
                }
                db.Rooms.Add(room);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(room);
        }

        // GET: Room/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // POST: Room/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public ActionResult Edit([Bind(Include = "RoomId,RoomName,ImagePath,PricePerNight,Status")] Room room, HttpPostedFileBase Image)
        {
            var existingRoom = db.Rooms.Include(r => r.BookingDetails).FirstOrDefault(r => r.RoomId == room.RoomId);

            if (existingRoom == null)
            {
                return HttpNotFound();
            }
            if (existingRoom.BookingDetails.Any())
            {
                ModelState.AddModelError("", "Cannot edit this room rightnow because it has associated bookings,try later.");
                return View(existingRoom);
            }
            // Handle image upload
            if (Image != null)
            {
                if (Image.ContentLength > 1 * 1024 * 1024)
                {
                    ModelState.AddModelError("Image", $"Upload limit is 1MB. Your file is {Image.ContentLength / (1 * 1024 * 1024)}MB");
                    return View(existingRoom);
                }
                string imagepath = $@"\Content\images\{Guid.NewGuid()}-{Image.FileName}";
                string filepath = Server.MapPath(imagepath);
                Image.SaveAs(filepath);             
                existingRoom.ImagePath = imagepath;
            }
            else
            {
                
                existingRoom.ImagePath = existingRoom.ImagePath;
            }

            existingRoom.RoomName = room.RoomName;
            existingRoom.PricePerNight = room.PricePerNight;
            existingRoom.Status = room.Status;

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Room/Delete/5
    
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // POST: Room/Delete/5
       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Room room = db.Rooms.Include(r => r.BookingDetails).FirstOrDefault(r => r.RoomId == id);

            if (room == null)
            {
                return HttpNotFound();
            }

            if (room.BookingDetails.Any())
            {
                ModelState.AddModelError("", "Cannot delete this room rightnow because it has associated bookings,try later.");

                return View("Delete", room);
            }

            db.Rooms.Remove(room);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Room/Checkout/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }

            // Set the status to false to make the room available
            room.Status = false;

            // Save the changes to the database
            db.Entry(room).State = EntityState.Modified;
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
