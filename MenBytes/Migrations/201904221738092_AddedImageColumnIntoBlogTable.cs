namespace MenBytes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedImageColumnIntoBlogTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Blogs", "blogImage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Blogs", "blogImage");
        }
    }
}
