using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202112082134)]
    public class AddRecurringExpense_202112082134 : Migration
    {
        public override void Up()
        {
            Create.Table("RecurringExpense")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Description").AsString(255)
                .WithColumn("Value").AsDecimal(10, 2)
                .WithColumn("InactiveAt").AsDateTime().Nullable()
                .WithColumn("CreditCardId").AsInt32().Nullable()
                .WithColumn("UserId").AsInt32();

            Create.ForeignKey()
                .FromTable("RecurringExpense").ForeignColumn("CreditCardId")
                .ToTable("CreditCard").PrimaryColumn("Id");

            Create.ForeignKey()
                .FromTable("RecurringExpense").ForeignColumn("UserId")
                .ToTable("User").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.Table("RecurringExpense");
        }
    }
}