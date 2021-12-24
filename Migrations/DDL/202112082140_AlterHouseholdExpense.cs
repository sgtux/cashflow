using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202112082140)]
    public class AlterHouseholdExpense_202112082140 : Migration
    {
        public override void Up()
        {
            Alter.Table("HouseholdExpense").AddColumn("Value").AsDecimal(10, 2).WithDefaultValue(0);
            Execute.Sql("ALTER TABLE \"HouseholdExpense\" RENAME COLUMN \"ShopName\" TO \"Description\"");
        }

        public override void Down()
        {
            Delete.Column("Value").FromTable("HouseholdExpense");
            Execute.Sql("ALTER TABLE \"HouseholdExpense\" RENAME COLUMN \"Description\" TO \"ShopName\"");
        }
    }
}