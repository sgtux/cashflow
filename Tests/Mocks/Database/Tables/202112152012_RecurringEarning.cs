using FluentMigrator;

namespace Cashflow.Tests.Mocks.Database.Tables
{
    [Migration(202112152012)]
    public class RecurringEarning : Migration
    {
        public override void Up()
        {
            Create.Table("RecurringEarning")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Description").AsString(255)
                .WithColumn("Value").AsDecimal(10, 2)
                .WithColumn("InactiveAt").AsDateTime().Nullable();

            Execute.Sql("ALTER TABLE RecurringEarning ADD COLUMN UserId INTEGER REFERENCES User(Id)");
        }

        public override void Down()
        {
        }
    }
}
