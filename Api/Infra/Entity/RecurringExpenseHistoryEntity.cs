using System;

namespace Cashflow.Api.Infra.Entity
{
    public class RecurringExpenseHistoryEntity : BaseEntity
    {
        public long Id { get; set; }

        public decimal PaidValue { get; set; }

        public DateTime Date { get; set; }

        public long RecurringExpenseId { get; set; }
    }
}