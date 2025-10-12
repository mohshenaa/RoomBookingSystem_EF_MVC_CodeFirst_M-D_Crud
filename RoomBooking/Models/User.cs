namespace RoomBooking.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public partial class User
    {
        // [Remote("CheckCode", "Account", ErrorMessage = "Code exists")]

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
