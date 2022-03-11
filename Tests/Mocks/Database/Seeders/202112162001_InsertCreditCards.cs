using FluentMigrator;

namespace Cashflow.Tests.Mocks.Database.Seeders
{
    [Migration(202112162001)]
    public class InsertCreditCards : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("CreditCard")
                .Row(new { Id = 1, Name = "Primeiro Cartão", UserId = 1, InvoiceDueDay = 20, InvoiceClosingDay = 10 })
                .Row(new { Id = 2, Name = "Segundo Cartão", UserId = 1, InvoiceDueDay = 20, InvoiceClosingDay = 10 })
                .Row(new { Id = 3, Name = "Terceiro Cartão", UserId = 2, InvoiceDueDay = 20, InvoiceClosingDay = 10 });
        }

        public override void Down()
        {
        }
    }
}