using System;
using System.Collections.Generic;
using System.Linq;
using Cashflow.Api.Extensions;

namespace Cashflow.Api.Infra.Entity
{
    public class RecurringExpense
    {
        public long Id { get; set; }

        public string Description { get; set; }

        public decimal Value { get; set; }

        public DateTime? InactiveAt { get; set; }

        public int? CreditCardId { get; set; }

        public CreditCard CreditCard { get; set; }

        public int UserId { get; set; }

        public List<RecurringExpenseHistory> History { get; set; }

        public bool Paid => History?.Any(p => p.Date.SameMonthYear(DateTime.Now)) ?? false;

        public void SortHistory()
        {
            if (History?.Any() ?? false)
                History = History.OrderBy(p => p.Date).ToList();
        }
    }
}