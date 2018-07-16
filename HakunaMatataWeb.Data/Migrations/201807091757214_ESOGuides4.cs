namespace HakunaMatataWeb.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ESOGuides4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ESOGuide",
                c => new
                    {
                        EsoGuideId = c.Int(nullable: false, identity: true),
                        GuideType = c.Int(nullable: false),
                        Title = c.String(maxLength: 30),
                        SubTitle = c.String(maxLength: 160),
                        Content = c.String(maxLength: 4000),
                        Author = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                        LastUpdatedDate = c.DateTime(nullable: false),
                        AuthorId = c.String(),
                    })
                .PrimaryKey(t => t.EsoGuideId);
            
            CreateTable(
                "dbo.ImageUrl",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ESOGuideId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ESOGuide", t => t.ESOGuideId, cascadeDelete: true)
                .Index(t => t.ESOGuideId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ImageUrl", "ESOGuideId", "dbo.ESOGuide");
            DropIndex("dbo.ImageUrl", new[] { "ESOGuideId" });
            DropTable("dbo.ImageUrl");
            DropTable("dbo.ESOGuide");
        }
    }
}
