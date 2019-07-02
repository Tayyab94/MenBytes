namespace MenBytes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingBlogTableIntoDataBase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Blogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                        postedBy = c.String(nullable: false),
                        blogMessage = c.String(nullable: false),
                        postedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Blogs");
        }
    }
}
