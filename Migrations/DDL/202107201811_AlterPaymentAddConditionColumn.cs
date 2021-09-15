using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202107201811)]
    public class AlterPaymentAddConditionColumn_202107201811 : Migration
    {
        public override void Up()
        {
            Alter.Table("Payment").AddColumn("Condition").AsInt16().NotNullable().WithDefaultValue(3);
        }

        public override void Down()
        {
            Delete.Column("Condition").FromTable("Payment");
        }
    }
}