namespace MenBytes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrderStatusOrderShippingAndOrrderTableIntoDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        userID = c.String(maxLength: 128),
                        createAt = c.DateTime(nullable: false),
                        totalBill = c.Double(nullable: false),
                        paymentType = c.String(),
                        orderStatusID = c.Int(nullable: false),
                        OrderShippingID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OrderShippings", t => t.OrderShippingID, cascadeDelete: true)
                .ForeignKey("dbo.OrderStatus", t => t.orderStatusID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.userID)
                .Index(t => t.userID)
                .Index(t => t.orderStatusID)
                .Index(t => t.OrderShippingID);
            
            CreateTable(
                "dbo.OrderShippings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        firstName = c.String(),
                        lastName = c.String(),
                        company_name = c.String(),
                        address = c.String(),
                        City = c.String(),
                        Country = c.String(),
                        email = c.String(),
                        phone = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrderStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        statusName = c.String(maxLength: 25),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "userID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Orders", "orderStatusID", "dbo.OrderStatus");
            DropForeignKey("dbo.Orders", "OrderShippingID", "dbo.OrderShippings");
            DropIndex("dbo.Orders", new[] { "OrderShippingID" });
            DropIndex("dbo.Orders", new[] { "orderStatusID" });
            DropIndex("dbo.Orders", new[] { "userID" });
            DropTable("dbo.OrderStatus");
            DropTable("dbo.OrderShippings");
            DropTable("dbo.Orders");
        }
    }
}
