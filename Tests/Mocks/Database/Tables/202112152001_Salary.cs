using FluentMigrator;

namespace Cashflow.Tests.Mocks.Database.Tables
{
    [Migration(202112152001)]
    public class Salary : Migration
    {
        public override void Up()
        {
            Create.Table("Salary")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Value").AsDecimal(10, 2)
                .WithColumn("StartDate").AsDateTime()
                .WithColumn("EndDate").AsDateTime().Nullable();

            Execute.Sql("ALTER TABLE Salary ADD COLUMN UserId INTEGER REFERENCES User(Id)");
        }

        public override void Down()
        {
        }
    }
}