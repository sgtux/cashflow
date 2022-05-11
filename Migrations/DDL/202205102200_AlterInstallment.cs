using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202205102200)]
    public class AlterInstallment_202205102200 : Migration
    {
        public override void Up()
        {
            Alter.Table("Installment").AddColumn("Exempt").AsBoolean().WithDefaultValue(false);
        }

        public override void Down() { }
    }
}