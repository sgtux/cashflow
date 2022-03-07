using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202203071200)]
    public class AlterPaymentRemoveInactiveAt_202203071200 : Migration
    {
        public override void Up()
        {
            Delete.Column("InactiveAt").FromTable("Payment");
        }

        public override void Down()
        {
        }
    }
}