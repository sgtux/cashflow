using FluentMigrator;

namespace Cashflow.Tests.Mocks.Database.Tables
{
    [Migration(202112152000)]
    public class User : Migration
    {
        public override void Up()
        {
            Create.Table("User")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Email").AsString(255).NotNullable()
                .WithColumn("Password").AsString(255).NotNullable()
                .WithColumn("CreatedAt").AsDateTime().NotNullable()
                .WithColumn("ExpenseLimit").AsDecimal().Nullable()
                .WithColumn("FuelExpenseLimit").AsDecimal().Nullable();
        }

        public override void Down()
        {
        }
    }
}