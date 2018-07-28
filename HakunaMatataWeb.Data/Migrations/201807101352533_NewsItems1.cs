namespace HakunaMatataWeb.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class NewsItems1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NewsItem",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Title = c.String(maxLength: 30),
                    SubTitle = c.String(maxLength: 160),
                    Content = c.String(maxLength: 4000),
                    Author = c.String(),
                    CreationDate = c.DateTime(nullable: false),
                    LastUpdatedDate = c.DateTime(nullable: false),
                    AuthorId = c.String(),
                })
                .PrimaryKey(t => t.Id);
        }

        public override void Down()
        {
            DropTable("dbo.NewsItem");
        }
    }
}