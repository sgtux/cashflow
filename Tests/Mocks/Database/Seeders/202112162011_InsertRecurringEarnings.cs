using System;
using FluentMigrator;

namespace Cashflow.Tests.Mocks.Database.Seeders
{
    [Migration(202112162011)]
    public class InsertRecurringEarnings : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("RecurringEarning")
                .Row(new { Id = 1, Description = "Salário", Value = 2000, UserId = 1 })
                .Row(new { Id = 2, Description = "Salário", Value = 2000, UserId = 2 })
                .Row(new { Id = 3, Description = "Salário", Value = 2000, UserId = 3 })
                .Row(new { Id = 4, Description = "Salário", Value = 2500, UserId = 4 })
                .Row(new { Id = 5, Description = "Freelance", Value = 1000, UserId = 1 })
                .Row(new { Id = 6, Description = "Salário Antigo", Value = 1500, UserId = 4, InactiveAt = new DateTime(2020, 1, 1) });
        }

        public override void Down()
        {
        }
    }
}
