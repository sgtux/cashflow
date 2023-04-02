using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202106141800)]
    public class AddUserTable : Migration
    {
        public override void Up()
        {
            Create.Table("User")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Email").AsString(255).NotNullable()
                .WithColumn("Password").AsString(255).Nullable()
                .WithColumn("CreatedAt").AsDateTime().NotNullable()
                .WithColumn("SpendingCeiling").AsDecimal().Nullable();
        }

        public override void Down()
        {
            Delete.Table("User");
        }
    }
}