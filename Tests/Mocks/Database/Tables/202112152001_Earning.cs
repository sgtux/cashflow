using FluentMigrator;

namespace Cashflow.Tests.Mocks.Database.Tables
{
    [Migration(202112152001)]
    public class Earning : Migration
    {
        public override void Up()
        {
            Create.Table("Earning")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Description").AsString(255)
                .WithColumn("Value").AsDecimal(10, 2)
                .WithColumn("Date").AsDateTime()
                .WithColumn("Type").AsInt16();

            Execute.Sql("ALTER TABLE Earning ADD COLUMN UserId INTEGER REFERENCES User(Id)");
        }

        public override void Down()
        {
        }
    }
}