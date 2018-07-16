namespace HakunaMatataWeb.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ESOGuides1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ImageUrl",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ESOGuide_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ESOGuide", t => t.ESOGuide_Id)
                .Index(t => t.ESOGuide_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ImageUrl", "ESOGuide_Id", "dbo.ESOGuide");
            DropIndex("dbo.ImageUrl", new[] { "ESOGuide_Id" });
            DropTable("dbo.ImageUrl");
        }
    }
}
