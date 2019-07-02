namespace MenBytes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_newColumnContactDateIntoContactUsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContactUs", "ContactDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ContactUs", "ContactDate");
        }
    }
}
