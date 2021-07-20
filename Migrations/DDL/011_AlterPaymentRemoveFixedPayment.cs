using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202107201830)]
    public class AlterPaymentRemoveFixedPayment_202107201830 : Migration
    {
        public override void Up()
        {
            Delete.Column("FixedPayment").FromTable("Payment");
        }

        public override void Down()
        {
            Alter.Table("Payment").AddColumn("FixedPayment").AsBoolean();
        }
    }
}