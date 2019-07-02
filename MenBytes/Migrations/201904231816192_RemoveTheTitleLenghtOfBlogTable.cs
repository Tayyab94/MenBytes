namespace MenBytes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveTheTitleLenghtOfBlogTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Blogs", "Title", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Blogs", "Title", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
