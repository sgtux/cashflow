using FluentMigrator;

namespace Cashflow.Tests.Mocks.Database.Tables
{
    [Migration(202112152013)]
    public class RecurringEarningHistory : Migration
    {
        public override void Up()
        {
            Create.Table("RecurringEarningHistory")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Value").AsDecimal(10, 2)
                .WithColumn("Date").AsDateTime();

            Execute.Sql("ALTER TABLE RecurringEarningHistory ADD COLUMN RecurringEarningId INTEGER REFERENCES RecurringEarning(Id)");
        }

        public override void Down()
        {
        }
    }
}
