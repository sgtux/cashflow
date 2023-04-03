using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202106141802)]
    public class AddCreditCardTable : Migration
    {
        public override void Up()
        {
            Create.Table("CreditCard")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Name").AsString(255).NotNullable()
                .WithColumn("UserId").AsInt32().NotNullable()
                .WithColumn("InvoiceClosingDay").AsInt16().WithDefaultValue(20)
                .WithColumn("InvoiceDueDay").AsInt16().WithDefaultValue(20);
        }

        public override void Down()
        {
            Delete.Table("CreditCard");
        }
    }
}