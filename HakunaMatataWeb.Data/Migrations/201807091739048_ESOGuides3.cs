namespace HakunaMatataWeb.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ESOGuides3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ImageUrl", "ESOGuide_Id", "dbo.ESOGuide");
            DropIndex("dbo.ImageUrl", new[] { "ESOGuide_Id" });
            DropTable("dbo.ESOGuide");
            DropTable("dbo.ImageUrl");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ImageUrl",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ESOGuide_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ESOGuide",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GuideType = c.Int(nullable: false),
                        Title = c.String(maxLength: 30),
                        SubTitle = c.String(maxLength: 160),
                        Content = c.String(maxLength: 4000),
                        Author = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdatedDate = c.DateTime(nullable: false),
                        AuthorId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.ImageUrl", "ESOGuide_Id");
            AddForeignKey("dbo.ImageUrl", "ESOGuide_Id", "dbo.ESOGuide", "Id");
        }
    }
}
