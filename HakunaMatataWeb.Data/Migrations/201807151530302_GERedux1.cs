namespace HakunaMatataWeb.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class GERedux1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GuildEvent", "IsUniqueEvent", c => c.Boolean(nullable: false));
            AddColumn("dbo.GuildEvent", "IsMonthly", c => c.Boolean(nullable: false));
            AddColumn("dbo.GuildEvent", "IsWeekly", c => c.Boolean(nullable: false));
            AddColumn("dbo.GuildEvent", "IsBiWeekly", c => c.Boolean(nullable: false));
            AddColumn("dbo.GuildEvent", "EventDayOfWeek", c => c.Int(nullable: false));
            AddColumn("dbo.GuildEvent", "EventDayOfMonth", c => c.Int(nullable: false));
            AddColumn("dbo.GuildEvent", "Content", c => c.String());
            AddColumn("dbo.GuildEvent", "LastUpdatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.GuildEvent", "FirstEventDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.GuildEvent", "LastEventDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.GuildEvent", "IsRecurring");
            DropColumn("dbo.GuildEvent", "EventDescription");
            DropColumn("dbo.GuildEvent", "EventDate");
        }

        public override void Down()
        {
            AddColumn("dbo.GuildEvent", "EventDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.GuildEvent", "EventDescription", c => c.String(maxLength: 1200));
            AddColumn("dbo.GuildEvent", "IsRecurring", c => c.Boolean(nullable: false));
            DropColumn("dbo.GuildEvent", "LastEventDate");
            DropColumn("dbo.GuildEvent", "FirstEventDate");
            DropColumn("dbo.GuildEvent", "LastUpdatedDate");
            DropColumn("dbo.GuildEvent", "Content");
            DropColumn("dbo.GuildEvent", "EventDayOfMonth");
            DropColumn("dbo.GuildEvent", "EventDayOfWeek");
            DropColumn("dbo.GuildEvent", "IsBiWeekly");
            DropColumn("dbo.GuildEvent", "IsWeekly");
            DropColumn("dbo.GuildEvent", "IsMonthly");
            DropColumn("dbo.GuildEvent", "IsUniqueEvent");
        }
    }
}