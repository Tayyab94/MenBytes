namespace MenBytes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Isfeatured_and_DiscrptionPropertiesIntoProductTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "ProductDescription", c => c.String());
            AddColumn("dbo.Products", "IsFeatured", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Products", "ProductName", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "ProductName", c => c.String());
            DropColumn("dbo.Products", "IsFeatured");
            DropColumn("dbo.Products", "ProductDescription");
        }
    }
}
