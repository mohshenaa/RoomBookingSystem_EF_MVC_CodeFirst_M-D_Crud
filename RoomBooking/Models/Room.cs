namespace RoomBooking.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Room
    {
     
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public string ImagePath { get; set; }
        public decimal PricePerNight { get; set; }
        public bool Status { get; set; }
    
        
        public virtual ICollection<BookingDetail> BookingDetails { get; set; }
    }
}
