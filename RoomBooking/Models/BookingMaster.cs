namespace RoomBooking.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class BookingMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookingId { get; set; }
        public string GuestName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime BookingDate { get; set; }
        public string ImagePath { get; set; }
        public virtual ICollection<BookingDetail> BookingDetails { get; set; }
    }
}
