using System;
using FluentMigrator;

namespace Cashflow.Tests.Mocks.Database.Seeders
{
    [Migration(202112162007)]
    public class InsertPaymentInstallments : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("Installment")
                .Row(new { Id = 1, Cost = 1500.5, Date = new DateTime(2019, 5, 1), PaymentId = 1, Number = 1 })
                .Row(new { Id = 2, Cost = 1500.5, Date = new DateTime(2019, 5, 1), PaymentId = 2, Number = 1 })
                .Row(new { Id = 3, Cost = 1500.5, Date = new DateTime(2019, 5, 1), PaymentId = 3, Number = 1 })
                .Row(new { Id = 4, Cost = 1500.5, Date = new DateTime(2019, 5, 1), PaymentId = 4, Number = 1 });

        }

        public override void Down()
        {
        }
    }
}