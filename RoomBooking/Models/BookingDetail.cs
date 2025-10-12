namespace RoomBooking.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class BookingDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookingDetailsId { get; set; }
        [ForeignKey("BookingMaster")]
        public int BookingId { get; set; }
        [ForeignKey("Room")]
        public int RoomId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public Nullable<int> StayingDays { get; set; }
        public decimal PricePerNight { get; set; }
        public Nullable<decimal> Bill { get; set; }
    
        public virtual BookingMaster BookingMaster { get; set; }
        public virtual Room Room { get; set; }
    }
}
