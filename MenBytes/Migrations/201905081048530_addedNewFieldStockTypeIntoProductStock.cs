namespace MenBytes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedNewFieldStockTypeIntoProductStock : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductStocks", "stockType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductStocks", "stockType");
        }
    }
}
