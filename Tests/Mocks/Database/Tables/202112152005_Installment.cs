using FluentMigrator;

namespace Cashflow.Tests.Mocks.Database.Tables
{
    [Migration(202112152005)]
    public class Installment : Migration
    {
        public override void Up()
        {
            Create.Table("Installment")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Value").AsDecimal(10, 2).NotNullable()
                .WithColumn("PaidValue").AsDecimal(10, 2).Nullable()
                .WithColumn("Number").AsInt32().NotNullable()
                .WithColumn("Date").AsDateTime().NotNullable()
                .WithColumn("PaidDate").AsDateTime().Nullable();

            Execute.Sql("ALTER TABLE Installment ADD COLUMN PaymentId INTEGER REFERENCES Payment(Id)");
        }

        public override void Down()
        {
        }
    }
}