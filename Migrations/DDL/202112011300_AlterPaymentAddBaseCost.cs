using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202112011300)]
    public class AlterPaymentAddBaseCost_202112011300 : Migration
    {
        public override void Up()
        {
            Alter.Table("Payment").AddColumn("BaseCost").AsDecimal(10, 2).WithDefaultValue(0);
        }

        public override void Down()
        {
            Delete.Column("BaseCost").FromTable("Payment"); ;
        }
    }
}