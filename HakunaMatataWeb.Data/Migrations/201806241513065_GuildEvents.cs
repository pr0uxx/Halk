namespace HakunaMatataWeb.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GuildEvents : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GuildEvent",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 60),
                        EventType = c.Int(nullable: false),
                        IsRecurring = c.Boolean(nullable: false),
                        EventDescription = c.String(maxLength: 1200),
                        UserId = c.String(),
                        EventMaster = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                        MinLevel = c.Int(nullable: false),
                        MaxLevel = c.Int(nullable: false),
                        Featured = c.Boolean(nullable: false),
                        EventDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.GuildEvent");
        }
    }
}
