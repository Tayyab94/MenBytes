namespace MenBytes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBlogCommentsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BlogsComments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Comment = c.String(nullable: false),
                        PostedDate = c.DateTime(nullable: false),
                        BlogID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Blogs", t => t.BlogID, cascadeDelete: true)
                .Index(t => t.BlogID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BlogsComments", "BlogID", "dbo.Blogs");
            DropIndex("dbo.BlogsComments", new[] { "BlogID" });
            DropTable("dbo.BlogsComments");
        }
    }
}
