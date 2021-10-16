using System;
using System.Collections.Generic;

namespace Cashflow.Api.Infra.Entity
{
    public class DailyExpenses
    {
        public long Id { get; set; }

        public string ShopName { get; set; }

        public DateTime Date { get; set; }

        public int UserId { get; set; }

        public List<DailyExpensesItem> Items { get; set; }
    }
}