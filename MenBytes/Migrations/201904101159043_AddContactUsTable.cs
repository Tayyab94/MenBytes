namespace MenBytes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddContactUsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ContactUs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        fullName = c.String(nullable: false, maxLength: 20),
                        Phone = c.String(nullable: false),
                        Subject = c.String(),
                        Message = c.String(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ContactUs");
        }
    }
}
