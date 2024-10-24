using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202410200000)]
    public class RenameUserColumn_202410200000 : Migration
    {
        public override void Up()
        {
            Rename.Column("SpendingCeiling").OnTable("User").To("ExpenseLimit");
        }

        public override void Down()
        {
            Rename.Column("ExpenseLimit").OnTable("User").To("SpendingCeiling");
        }
    }
}