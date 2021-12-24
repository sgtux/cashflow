using FluentMigrator;

namespace Cashflow.Tests.Mocks.Database.Seeders
{
    [Migration(202112162005)]
    public class InsertPaymentsTypes : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("PaymentType")
                .Row(new { Id = 1, Description = "Gastos", In = false })
                .Row(new { Id = 2, Description = "Ganhos", In = true })
                .Row(new { Id = 3, Description = "Aportes", In = false })
                .Row(new { Id = 4, Description = "Dividendos", In = true });
        }

        public override void Down()
        {
        }
    }
}