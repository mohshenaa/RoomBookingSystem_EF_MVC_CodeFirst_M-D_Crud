using RoomBooking.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RoomBooking.ViewModel
{
    public class BookingVM
    {
        public int BookingId { get; set; }

        [Required]
        [Display(Name = "Guest Name")]
        public string GuestName { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        //[Required]
        [Display(Name = "Guest Image")]
        public string ImagePath { get; set; }

        public HttpPostedFileBase GuestImageFile { get; set; }

        [ScaffoldColumn(false)]
        public decimal Bill { get; set; }
        public string operation { get; set; } = "save";

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}",
           ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, HtmlEncode = true, NullDisplayText = "Select Booking Date")]
        [DisplayName("Booking Date")]
        [Required(ErrorMessage = "Booking Date is required.")]
        public System.DateTime BookingDate { get; set; } = DateTime.Now;

        public List<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();

        public BookingVM()
        { }
        public BookingVM(BookingMaster master)
        {
            this.BookingId = master.BookingId;
            this.GuestName = master.GuestName;
            this.Phone = master.Phone;
            this.Email = master.Email;
            this.ImagePath = master.ImagePath;
            this.BookingDate = master.BookingDate;
            //this.Bill = master.Bill;
            this.BookingDetails = master.BookingDetails.ToList();
        }
        public BookingMaster Convert()
        {
            BookingMaster model = new BookingMaster();
            model.BookingId = this.BookingId;
            model.GuestName = this.GuestName;
            model.Phone = this.Phone;
            model.Email = this.Email;
            model.ImagePath = this.ImagePath;
            model.BookingDate = this.BookingDate;
           // model.Bill = this.Bill;
            model.BookingDetails = this.BookingDetails;

            return model;
        }


    }
}