using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202112011200)]
    public class AlterPaymentChangeActiveColumn_202112011200 : Migration
    {
        public override void Up()
        {
            Alter.Table("Payment").AddColumn("InactiveAt").AsDateTime().Nullable();
            Delete.Column("Active").FromTable("Payment");
        }

        public override void Down()
        {
            Delete.Column("InactiveAt").FromTable("Payment");
            Alter.Table("Payment").AddColumn("Active").AsBoolean().WithDefaultValue(true);
        }
    }
}