namespace MenBytes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_isDeleteAnd_ProductOrderDetailIntoProductTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "isDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "isDeleted");
        }
    }
}
