namespace HakunaMatataWeb.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ESOGuides5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ImageUrl", "Uri", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ImageUrl", "Uri");
        }
    }
}
