using System;
using FluentMigrator;

namespace Cashflow.Tests.Mocks.Database.Seeders
{
    [Migration(202112162007)]
    public class InsertPaymentInstallments : Migration
    {
        public override void Up()
        {
            var date = DateTime.Now.AddMonths(-3);
            Insert.IntoTable("Installment")
                .Row(new { Id = 1, Cost = 1500.5, Date = new DateTime(2019, 5, 1), PaymentId = 1, Number = 1 })
                .Row(new { Id = 2, Cost = 1500.5, Date = new DateTime(2019, 5, 1), PaymentId = 2, Number = 1 })
                .Row(new { Id = 3, Cost = 1500.5, Date = new DateTime(2019, 5, 1), PaymentId = 3, Number = 1 })
                .Row(new { Id = 4, Cost = 1500.5, Date = new DateTime(2019, 5, 1), PaymentId = 4, Number = 1 })
                .Row(new { Id = 5, Cost = 2000, Date = date, PaymentId = 5, Number = 1 })
                .Row(new { Id = 6, Cost = 2000, Date = date.AddMonths(1), PaymentId = 5, Number = 2 })
                .Row(new { Id = 7, Cost = 2000, Date = date.AddMonths(2), PaymentId = 5, Number = 3 })
                .Row(new { Id = 8, Cost = 2000, Date = date.AddMonths(3), PaymentId = 5, Number = 4 })
                .Row(new { Id = 9, Cost = 2000, Date = date.AddMonths(4), PaymentId = 5, Number = 5 })
                .Row(new { Id = 10, Cost = 2000, Date = date.AddMonths(5), PaymentId = 5, Number = 6 });

        }

        public override void Down()
        {
        }
    }
}