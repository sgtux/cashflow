using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202106141823)]
    public class InsertPaymentTypes : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("PaymentType")
                .Row(new { Id = 1, Description = "Ganho" })
                .Row(new { Id = 2, Description = "Renda" })
                .Row(new { Id = 3, Description = "Despesa" })
                .Row(new { Id = 4, Description = "Crédito" })
                .Row(new { Id = 5, Description = "Lucro" });
        }

        public override void Down()
        {
        }
    }
}