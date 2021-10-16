using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202109151300)]
    public class AddDailyExpensesTable : Migration
    {
        public override void Up()
        {
            Create.Table("DailyExpenses")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("ShopName").AsString(255).NotNullable()
                .WithColumn("Date").AsDateTime().NotNullable()
                .WithColumn("UserId").AsInt32().NotNullable();

            Create.ForeignKey()
                .FromTable("DailyExpenses").ForeignColumn("UserId")
                .ToTable("User").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.Table("DailyExpenses");
        }
    }
}