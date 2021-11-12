using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202111111534)]
    public class AlterPaymentAddActiveColumn_202111111534 : Migration
    {
        public override void Up()
        {
            Alter.Table("Payment").AddColumn("Active").AsBoolean().WithDefaultValue(true);
        }

        public override void Down()
        {
            Delete.Column("Active").FromTable("Payment");
        }
    }
}