using FluentMigrator;

namespace Cashflow.Tests.Mocks.Database.Tables
{
    [Migration(202112152004)]
    public class Payment : Migration
    {
        public override void Up()
        {
            Create.Table("Payment")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Description").AsString(255).NotNullable()
                .WithColumn("Date").AsDateTime()
                .WithColumn("Type").AsInt32().NotNullable();

            Execute.Sql("ALTER TABLE Payment ADD COLUMN UserId INTEGER REFERENCES User(Id)");
            Execute.Sql("ALTER TABLE Payment ADD COLUMN CreditCardId INTEGER REFERENCES CreditCard(Id)");
        }

        public override void Down()
        {
        }
    }
}