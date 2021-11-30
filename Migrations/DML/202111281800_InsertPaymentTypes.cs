using FluentMigrator;

namespace Cashflow.Migrations.DDL
{
    [Migration(202111281800)]
    public class InsertPaymentTypes_202111281800 : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("PaymentType")
                .Row(new { Id = 5, Description = "Financiamento", In = false })
                .Row(new { Id = 6, Description = "Educação", In = false })
                .Row(new { Id = 7, Description = "Empréstimo", In = false });
        }

        public override void Down()
        {
        }
    }
}