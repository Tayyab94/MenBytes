namespace MenBytes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedProductStockTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductStocks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        productId = c.Int(nullable: false),
                        productPreviourQuantity = c.Int(nullable: false),
                        productUpdatedQuantity = c.Int(nullable: false),
                        createdDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.productId, cascadeDelete: true)
                .Index(t => t.productId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductStocks", "productId", "dbo.Products");
            DropIndex("dbo.ProductStocks", new[] { "productId" });
            DropTable("dbo.ProductStocks");
        }
    }
}
