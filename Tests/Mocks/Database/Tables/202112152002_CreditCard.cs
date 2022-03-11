using FluentMigrator;

namespace Cashflow.Tests.Mocks.Database.Tables
{
    [Migration(202112152002)]
    public class CreditCard : Migration
    {
        public override void Up()
        {
            Create.Table("CreditCard")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Name").AsString(255)
                .WithColumn("InvoiceClosingDay").AsInt16()
                .WithColumn("InvoiceDueDay").AsInt16();

            Execute.Sql("ALTER TABLE CreditCard ADD COLUMN UserId INTEGER REFERENCES User(Id)");
        }

        public override void Down()
        {
        }
    }
}