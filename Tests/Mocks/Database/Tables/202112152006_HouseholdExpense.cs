using FluentMigrator;

namespace Cashflow.Tests.Mocks.Database.Tables
{
    [Migration(202112152006)]
    public class HouseholdExpense : Migration
    {
        public override void Up()
        {
            Create.Table("HouseholdExpense")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Description").AsString(255).NotNullable()
                .WithColumn("Date").AsDateTime().NotNullable()
                .WithColumn("Value").AsDecimal(10, 2);

            Execute.Sql("ALTER TABLE HouseholdExpense ADD COLUMN UserId INTEGER REFERENCES User(Id)");
        }

        public override void Down()
        {
        }
    }
}