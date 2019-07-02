namespace MenBytes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProductAndProductImageTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductImages",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ImagePath = c.String(),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProductName = c.String(),
                        Quantity = c.Int(nullable: false),
                        salePrice = c.Single(nullable: false),
                        purchasePrice = c.Single(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        CompanyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .Index(t => t.CategoryId)
                .Index(t => t.CompanyId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductImages", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Products", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.Products", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Products", new[] { "CompanyId" });
            DropIndex("dbo.Products", new[] { "CategoryId" });
            DropIndex("dbo.ProductImages", new[] { "ProductId" });
            DropTable("dbo.Products");
            DropTable("dbo.ProductImages");
        }
    }
}
