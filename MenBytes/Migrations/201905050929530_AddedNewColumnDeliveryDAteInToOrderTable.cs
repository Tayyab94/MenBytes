namespace MenBytes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedNewColumnDeliveryDAteInToOrderTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "DeliveryDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "DeliveryDate");
        }
    }
}
