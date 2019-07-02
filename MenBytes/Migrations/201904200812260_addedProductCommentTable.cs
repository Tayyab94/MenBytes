namespace MenBytes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedProductCommentTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductComments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Comment = c.String(nullable: false),
                        PostedDate = c.DateTime(nullable: false),
                        ProductID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.ProductID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductComments", "ProductID", "dbo.Products");
            DropIndex("dbo.ProductComments", new[] { "ProductID" });
            DropTable("dbo.ProductComments");
        }
    }
}
