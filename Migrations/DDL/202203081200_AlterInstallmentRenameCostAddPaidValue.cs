using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202203081200)]
    public class AlterInstallmentRenameCostAddPaidValue_202203081200 : Migration
    {
        public override void Up()
        {
            Execute.Sql("ALTER TABLE \"Installment\" RENAME COLUMN \"Cost\" TO \"Value\"");
            Alter.Table("Installment").AddColumn("PaidValue").AsDecimal(10, 2).Nullable();
        }

        public override void Down() { }
    }
}