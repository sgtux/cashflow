using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202109151300)]
    public class AddMarketExpensesTable : Migration
    {
        public override void Up()
        {
            Create.Table("MarketExpenses")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("MarketName").AsString(255).NotNullable()
                .WithColumn("Date").AsDateTime().NotNullable()
                .WithColumn("UserId").AsInt32().NotNullable();

            Create.ForeignKey()
                .FromTable("MarketExpenses").ForeignColumn("UserId")
                .ToTable("User").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.Table("MarketExpenses");
        }
    }
}