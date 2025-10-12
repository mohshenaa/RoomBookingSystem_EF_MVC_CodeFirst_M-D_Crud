namespace RoomBooking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class start : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BookingMasters", "ImagePath", c => c.String());
            AlterColumn("dbo.Users", "Username", c => c.String());
            AlterColumn("dbo.Users", "Password", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Password", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "Username", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.BookingMasters", "ImagePath");
        }
    }
}
