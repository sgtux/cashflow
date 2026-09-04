using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202609030004)]
    public class DropEarningTypeColumn_202609030004 : Migration
    {
        // After MigrateMonthlyEarnings every remaining Earning is a one-off entry,
        // so the Type column (EarningType) is no longer used.
        public override void Up()
        {
            Delete.Column("Type").FromTable("Earning");
        }

        public override void Down()
        {
            Alter.Table("Earning").AddColumn("Type").AsInt16().Nullable();
        }
    }
}
