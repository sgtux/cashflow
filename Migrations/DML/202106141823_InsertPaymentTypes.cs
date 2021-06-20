using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202106141823)]
    public class InsertPaymentTypes : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("PaymentType")
                .Row(new { Id = 1, Description = "Despesa", In = false })
                .Row(new { Id = 2, Description = "Renda", In = true })
                .Row(new { Id = 3, Description = "Ganho", In = true })
                .Row(new { Id = 4, Description = "Crédito", In = true })
                .Row(new { Id = 5, Description = "Lucro", In = true });
        }

        public override void Down()
        {
        }
    }
}