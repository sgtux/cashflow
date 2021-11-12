using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202111111936)]
    public class AlterPaymentRemoveInvoiceColumn_202111111936 : Migration
    {
        public override void Up()
        {
            Delete.Column("Invoice").FromTable("Payment");
        }

        public override void Down()
        {
            Alter.Table("Payment").AddColumn("Invoice").AsBoolean().WithDefaultValue(false);
        }
    }
}