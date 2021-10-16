using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202109151301)]
    public class AddDailyExpensesItemTable : Migration
    {
        public override void Up()
        {
            Create.Table("DailyExpensesItem")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("ItemName").AsString(255).NotNullable()
                .WithColumn("Price").AsDecimal(10, 2).NotNullable()
                .WithColumn("DailyExpensesId").AsInt32().NotNullable();

            Create.ForeignKey()
                .FromTable("DailyExpensesItem").ForeignColumn("DailyExpensesId")
                .ToTable("DailyExpenses").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.Table("DailyExpensesItem");
        }
    }
}