using System;

namespace Cashflow.Api.Infra.Entity
{
    public class RecurringExpenseHistory
    {
        public long Id { get; set; }

        public decimal PaidValue { get; set; }

        public DateTime Date { get; set; }

        public int RecurringExpenseId { get; set; }
    }
}