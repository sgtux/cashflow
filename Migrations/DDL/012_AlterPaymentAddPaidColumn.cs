using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202107201900)]
    public class AlterPaymentAddPaidColumn_202107201900 : Migration
    {
        public override void Up()
        {
            Alter.Table("Payment").AddColumn("Paid").AsBoolean().NotNullable().WithDefaultValue(false);
        }

        public override void Down()
        {
            Delete.Column("Paid").FromTable("Payment");
        }
    }
}