using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202610240002)]
    public class AddEarningTypeColumn_202610240002 : Migration
    {
        public override void Up()
        {
            Alter.Table("Earning").AddColumn("Type").AsInt32().NotNullable().WithDefaultValue(20);
            Alter.Table("RecurringEarning").AddColumn("Type").AsInt32().NotNullable().WithDefaultValue(20);
        }

        public override void Down()
        {
            Delete.Column("Type").FromTable("Earning");
            Delete.Column("Type").FromTable("RecurringEarning");
        }
    }
}
