using System;
using Cashflow.Api.Infra.Entity;
using Cashflow.Tests.Extensions;
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
                .Row(new Installment() { Id = 1, Value = 1500.5M, Date = new DateTime(2019, 5, 1), PaymentId = 1, Number = 1 }.ToSqliteEntity())
                .Row(new Installment() { Id = 2, Value = 1500.5M, Date = new DateTime(2019, 5, 1), PaymentId = 2, Number = 1 }.ToSqliteEntity())
                .Row(new Installment() { Id = 3, Value = 1500.5M, Date = new DateTime(2019, 5, 1), PaymentId = 3, Number = 1 }.ToSqliteEntity())
                .Row(new Installment() { Id = 4, Value = 1500.5M, Date = new DateTime(2019, 5, 1), PaymentId = 4, Number = 1 }.ToSqliteEntity())
                .Row(new Installment() { Id = 5, Value = 2000, Date = date, PaidDate = date, PaidValue = 2000, PaymentId = 5, Number = 1 }.ToSqliteEntity())
                .Row(new Installment() { Id = 6, Value = 2000, Date = date.AddMonths(1), PaidDate = date.AddMonths(1), PaidValue = 2000, PaymentId = 5, Number = 2 }.ToSqliteEntity())
                .Row(new Installment() { Id = 7, Value = 2000, Date = date.AddMonths(2), PaidDate = date.AddMonths(2), PaidValue = 2000, PaymentId = 5, Number = 3 }.ToSqliteEntity())
                .Row(new Installment() { Id = 8, Value = 2000, Date = date.AddMonths(3), PaidDate = date.AddMonths(3), PaidValue = 1500, PaymentId = 5, Number = 4 }.ToSqliteEntity())
                .Row(new Installment() { Id = 9, Value = 2000, Date = date.AddMonths(4), PaymentId = 5, Number = 5 }.ToSqliteEntity())
                .Row(new Installment() { Id = 10, Value = 2000, Date = date.AddMonths(5), PaymentId = 5, Number = 6 }.ToSqliteEntity());

        }

        public override void Down()
        {
        }
    }
}