
namespace RoomBooking.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class HotelDBEntities : DbContext
    {
        public HotelDBEntities()
            : base("name=HotelDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
           
        }
    
        public virtual DbSet<BookingDetail> BookingDetails { get; set; }
        public virtual DbSet<BookingMaster> BookingMasters { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
