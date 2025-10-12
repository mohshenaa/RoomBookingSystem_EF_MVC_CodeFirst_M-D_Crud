using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RoomBooking.ViewModel
{
    public class BookingDateGroup
    {
        [DataType(DataType.Date)]
        public DateTime? BookingDate { get; set; }
        public int BookingCount { get; set; }
    }
}