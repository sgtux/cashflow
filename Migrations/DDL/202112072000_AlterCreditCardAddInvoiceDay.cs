using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202112072000)]
    public class AlterCreditCardAddInvoiceDay_202112072000 : Migration
    {
        public override void Up()
        {
            Alter.Table("CreditCard").AddColumn("InvoiceDay").AsInt16().WithDefaultValue(20);
        }

        public override void Down()
        {
            Delete.Column("InvoiceDay").FromTable("CreditCard"); ;
        }
    }
}