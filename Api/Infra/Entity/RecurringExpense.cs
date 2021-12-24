using System;
using System.Collections.Generic;

namespace Cashflow.Api.Infra.Entity
{
    public class RecurringExpense
    {
        public long Id { get; set; }

        public string Description { get; set; }

        public decimal Value { get; set; }

        public DateTime? InactiveAt { get; set; }

        public int? CreditCardId { get; set; }

        public int UserId { get; set; }

        public List<RecurringExpenseHistory> History { get; set; }
    }
}