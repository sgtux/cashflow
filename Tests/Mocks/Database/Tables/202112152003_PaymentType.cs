using FluentMigrator;

namespace Cashflow.Tests.Mocks.Database.Tables
{
    [Migration(202112152003)]
    public class PaymentType : Migration
    {
        public override void Up()
        {
            Create.Table("PaymentType")
                .WithColumn("Id").AsInt32().PrimaryKey()
                .WithColumn("Description").AsString(100).NotNullable()
                .WithColumn("In").AsBoolean().NotNullable();
        }

        public override void Down()
        {
        }
    }
}