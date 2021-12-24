using FluentMigrator;

namespace Cashflow.Tests.Mocks.Database.Tables
{
    [Migration(202112152009)]
    public class RemainingBalance : Migration
    {
        public override void Up()
        {
            Create.Table("RemainingBalance")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Value").AsDecimal(10, 2).NotNullable()
                .WithColumn("Month").AsInt16().NotNullable()
                .WithColumn("Year").AsInt16().NotNullable();

            Execute.Sql("ALTER TABLE RemainingBalance ADD COLUMN UserId INTEGER REFERENCES User(Id)");
        }

        public override void Down()
        {
        }
    }
}