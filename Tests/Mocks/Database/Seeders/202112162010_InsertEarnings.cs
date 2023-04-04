using System;
using Cashflow.Api.Enums;
using FluentMigrator;

namespace Cashflow.Tests.Mocks.Database.Seeders
{
    [Migration(202112162010)]
    public class InsertEarnings : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("Earning")
                .Row(new { Id = 1, Description = "Salário", Value = 500, UserId = 1, Date = new DateTime(2019, 5, 1), Type = (int)EarningType.Monthy })
                .Row(new { Id = 2, Description = "Salário", Value = 500, UserId = 1, Date = new DateTime(2019, 6, 1), Type = (int)EarningType.Monthy })
                .Row(new { Id = 3, Description = "Salário", Value = 500, UserId = 1, Date = new DateTime(2019, 7, 1), Type = (int)EarningType.Monthy })
                .Row(new { Id = 4, Description = "Salário", Value = 500, UserId = 1, Date = new DateTime(2019, 8, 1), Type = (int)EarningType.Monthy })
                .Row(new { Id = 5, Description = "Salário", Value = 500, UserId = 1, Date = new DateTime(2019, 9, 1), Type = (int)EarningType.Monthy })
                .Row(new { Id = 6, Description = "Salário", Value = 500, UserId = 1, Date = new DateTime(2019, 10, 1), Type = (int)EarningType.Monthy })

                .Row(new { Id = 7, Description = "Salário", Value = 800, UserId = 1, Date = new DateTime(2019, 11, 1), Type = (int)EarningType.Monthy })
                .Row(new { Id = 8, Description = "Salário", Value = 800, UserId = 1, Date = new DateTime(2019, 12, 1), Type = (int)EarningType.Monthy })

                .Row(new { Id = 9, Description = "Salário", Value = 1200, UserId = 1, Date = new DateTime(2020, 1, 1), Type = (int)EarningType.Monthy })
                .Row(new { Id = 10, Description = "Salário", Value = 1200, UserId = 1, Date = new DateTime(2020, 2, 1), Type = (int)EarningType.Monthy })
                .Row(new { Id = 11, Description = "Salário", Value = 1200, UserId = 1, Date = new DateTime(2020, 3, 1), Type = (int)EarningType.Monthy })
                .Row(new { Id = 12, Description = "Salário", Value = 1200, UserId = 1, Date = new DateTime(2020, 4, 1), Type = (int)EarningType.Monthy })
                .Row(new { Id = 13, Description = "Salário", Value = 1200, UserId = 1, Date = new DateTime(2020, 5, 1), Type = (int)EarningType.Monthy })
                .Row(new { Id = 14, Description = "Salário", Value = 1200, UserId = 1, Date = new DateTime(2020, 6, 1), Type = (int)EarningType.Monthy })

                .Row(new { Id = 15, Description = "Salário", Value = 1500, UserId = 1, Date = new DateTime(2020, 7, 1), Type = (int)EarningType.Monthy })
                .Row(new { Id = 16, Description = "Salário", Value = 1500, UserId = 1, Date = new DateTime(2020, 8, 1), Type = (int)EarningType.Monthy })
                .Row(new { Id = 17, Description = "Salário", Value = 1500, UserId = 1, Date = new DateTime(2020, 9, 1), Type = (int)EarningType.Monthy })
                .Row(new { Id = 18, Description = "Salário", Value = 1500, UserId = 1, Date = new DateTime(2020, 10, 1), Type = (int)EarningType.Monthy })
                .Row(new { Id = 19, Description = "Salário", Value = 1500, UserId = 1, Date = new DateTime(2020, 11, 1), Type = (int)EarningType.Monthy })
                .Row(new { Id = 20, Description = "Salário", Value = 1500, UserId = 1, Date = new DateTime(2020, 12, 1), Type = (int)EarningType.Monthy })
                .Row(new { Id = 21, Description = "Salário", Value = 2000, UserId = 1, Date = DateTime.Now.AddMonths(-2), Type = (int)EarningType.Monthy })
                .Row(new { Id = 22, Description = "Salário", Value = 2000, UserId = 1, Date = DateTime.Now.AddMonths(-1), Type = (int)EarningType.Monthy })
                .Row(new { Id = 23, Description = "Salário", Value = 2000, UserId = 1, Date = DateTime.Now, Type = (int)EarningType.Monthy })

                .Row(new { Id = 24, Description = "Salário", Value = 500, UserId = 2, Date = new DateTime(2019, 5, 1), Type = (int)EarningType.Monthy })
                .Row(new { Id = 25, Description = "Salário", Value = 800, UserId = 2, Date = new DateTime(2019, 11, 1), Type = (int)EarningType.Monthy })
                .Row(new { Id = 26, Description = "Salário", Value = 1200, UserId = 2, Date = new DateTime(2020, 1, 1), Type = (int)EarningType.Monthy })
                .Row(new { Id = 27, Description = "Salário", Value = 1500, UserId = 2, Date = new DateTime(2020, 7, 1), Type = (int)EarningType.Monthy })
                .Row(new { Id = 28, Description = "Salário", Value = 2000, UserId = 2, Date = new DateTime(2020, 12, 1), Type = (int)EarningType.Monthy })
                .Row(new { Id = 29, Description = "Salário", Value = 2000, UserId = 2, Date = DateTime.Now.AddMonths(-2), Type = (int)EarningType.Monthy })
                .Row(new { Id = 30, Description = "Salário", Value = 2000, UserId = 2, Date = DateTime.Now.AddMonths(-1), Type = (int)EarningType.Monthy })
                .Row(new { Id = 31, Description = "Salário", Value = 2000, UserId = 2, Date = DateTime.Now, Type = (int)EarningType.Monthy })


                .Row(new { Id = 32, Description = "Salário", Value = 2000, UserId = 3, Date = new DateTime(2020, 12, 1), Type = (int)EarningType.Monthy })
                .Row(new { Id = 33, Description = "Salário", Value = 2500, UserId = 4, Date = DateTime.Now.AddMonths(-2), Type = (int)EarningType.Monthy })
                .Row(new { Id = 34, Description = "Salário", Value = 2500, UserId = 4, Date = DateTime.Now.AddMonths(-1), Type = (int)EarningType.Monthy })
                .Row(new { Id = 35, Description = "Salário", Value = 2500, UserId = 4, Date = DateTime.Now, Type = (int)EarningType.Monthy });
        }

        public override void Down()
        {
        }
    }
}