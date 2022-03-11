using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202203102200)]
    public class AlterCreditCard_202203102200 : Migration
    {
        public override void Up()
        {
            Execute.Sql("ALTER TABLE \"CreditCard\" RENAME COLUMN \"InvoiceDay\" TO \"InvoiceClosingDay\"");
            Alter.Table("CreditCard").AddColumn("InvoiceDueDay").AsInt16().WithDefaultValue(20);
        }

        public override void Down() { }
    }
}