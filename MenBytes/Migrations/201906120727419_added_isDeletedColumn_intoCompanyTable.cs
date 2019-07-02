namespace MenBytes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_isDeletedColumn_intoCompanyTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "isDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Companies", "isDeleted");
        }
    }
}
