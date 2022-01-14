using System;
using FluentMigrator;

namespace Cashflow.Tests.Mocks.Database.Seeders
{
    [Migration(202112162006)]
    public class InsertPayments : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("Payment")
                .Row(new { Id = 1, Description = "Payment 1", Date = new DateTime(2019, 5, 1), BaseCost = 55.5, UserId = 1, CreditCardId = 1, Type = 1, Condition = 3 })
                .Row(new { Id = 2, Description = "Payment 2", Date = new DateTime(2019, 10, 1), BaseCost = 55.5, UserId = 1, CreditCardId = 1, Type = 1, Condition = 3 })
                .Row(new { Id = 3, Description = "Payment 3", Date = new DateTime(2019, 12, 1), BaseCost = 55.5, UserId = 1, CreditCardId = 1, Type = 1, Condition = 3 })
                .Row(new { Id = 4, Description = "Payment 4", Date = new DateTime(2019, 9, 1), BaseCost = 55.5, UserId = 2, CreditCardId = 1, Type = 1, Condition = 3 })
                .Row(new { Id = 5, Description = "Payment 5", Date = new DateTime(2019, 9, 1), BaseCost = 55.5, UserId = 4, Type = 1, Condition = 3 });
        }

        public override void Down()
        {
        }
    }
}