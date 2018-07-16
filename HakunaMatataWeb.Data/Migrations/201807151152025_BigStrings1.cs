namespace HakunaMatataWeb.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BigStrings1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ESOGuide", "Content", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ESOGuide", "Content", c => c.String(maxLength: 4000));
        }
    }
}
