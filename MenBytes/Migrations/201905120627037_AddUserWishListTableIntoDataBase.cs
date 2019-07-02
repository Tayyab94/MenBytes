namespace MenBytes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserWishListTableIntoDataBase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserWishLists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        user_id = c.String(maxLength: 128),
                        product_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.product_id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.user_id)
                .Index(t => t.user_id)
                .Index(t => t.product_id);
            
            DropColumn("dbo.Products", "Quantity");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Quantity", c => c.Int(nullable: false));
            DropForeignKey("dbo.UserWishLists", "user_id", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserWishLists", "product_id", "dbo.Products");
            DropIndex("dbo.UserWishLists", new[] { "product_id" });
            DropIndex("dbo.UserWishLists", new[] { "user_id" });
            DropTable("dbo.UserWishLists");
        }
    }
}
