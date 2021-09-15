using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202109151301)]
    public class AddMarketExpensesTable : Migration
    {
        public override void Up()
        {
            Create.Table("MarketItems")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Name").AsString(255).NotNullable()
                .WithColumn("Price").AsDecimal(10, 2).NotNullable()
                .WithColumn("MarketExpensesId").AsInt32().NotNullable();

            Create.ForeignKey()
                .FromTable("MarketItems").ForeignColumn("MarketExpensesId")
                .ToTable("MarketExpenses").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.Table("MarketItems");
        }
    }
}