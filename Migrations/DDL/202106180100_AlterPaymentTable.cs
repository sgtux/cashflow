using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202106180100)]
    public class AlterPaymentTable_202106180100 : Migration
    {
        public override void Up()
        {
            Alter.Table("Payment").AlterColumn("CreditCardId").AsInt32().Nullable();
        }

        public override void Down()
        {
        }
    }
}