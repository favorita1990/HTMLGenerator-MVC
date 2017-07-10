namespace HTMLGeneratorMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Style_Id", "dbo.Styles");
            DropIndex("dbo.AspNetUsers", new[] { "Style_Id" });
            CreateIndex("dbo.Styles", "UserId");
            AddForeignKey("dbo.Styles", "UserId", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.AspNetUsers", "Style_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Style_Id", c => c.Int());
            DropForeignKey("dbo.Styles", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Styles", new[] { "UserId" });
            CreateIndex("dbo.AspNetUsers", "Style_Id");
            AddForeignKey("dbo.AspNetUsers", "Style_Id", "dbo.Styles", "Id");
        }
    }
}
