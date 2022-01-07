using FluentMigrator;

namespace Cashflow.Tests.Mocks.Database.Tables
{
    [Migration(202112152010)]
    public class RecurringExpense : Migration
    {
        public override void Up()
        {
            Create.Table("RecurringExpense")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Description").AsString(255)
                .WithColumn("Value").AsDecimal(10, 2)
                .WithColumn("InactiveAt").AsDateTime().Nullable();

            Execute.Sql("ALTER TABLE RecurringExpense ADD COLUMN CreditCardId INTEGER REFERENCES CreditCard(Id)");
            Execute.Sql("ALTER TABLE RecurringExpense ADD COLUMN UserId INTEGER REFERENCES User(Id)");
        }

        public override void Down()
        {
        }
    }
}