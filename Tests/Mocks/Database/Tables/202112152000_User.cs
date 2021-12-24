using FluentMigrator;

namespace Cashflow.Tests.Config.DatabaseSeeders
{
    [Migration(202112152000)]
    public class User : Migration
    {
        public override void Up()
        {
            Create.Table("User")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("NickName").AsString(255).NotNullable()
                .WithColumn("Password").AsString(255).NotNullable()
                .WithColumn("CreatedAt").AsDateTime().NotNullable();
        }

        public override void Down()
        {
        }
    }
}