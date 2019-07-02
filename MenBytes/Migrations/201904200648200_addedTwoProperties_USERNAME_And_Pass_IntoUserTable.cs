namespace MenBytes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedTwoProperties_USERNAME_And_Pass_IntoUserTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "user_Name", c => c.String(nullable: false));
            AddColumn("dbo.AspNetUsers", "accountPass", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "accountPass");
            DropColumn("dbo.AspNetUsers", "user_Name");
        }
    }
}
