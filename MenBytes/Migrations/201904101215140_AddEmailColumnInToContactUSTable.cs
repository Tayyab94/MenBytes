namespace MenBytes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmailColumnInToContactUSTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContactUs", "Email", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ContactUs", "Email");
        }
    }
}
