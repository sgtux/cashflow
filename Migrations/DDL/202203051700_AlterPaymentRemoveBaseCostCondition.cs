using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202203051700)]
    public class AlterPaymentRemoveBaseCostCondition_202203051700 : Migration
    {
        public override void Up()
        {
            Delete.Column("BaseCost").FromTable("Payment");
            Delete.Column("Condition").FromTable("Payment");
        }

        public override void Down()
        {
        }
    }
}