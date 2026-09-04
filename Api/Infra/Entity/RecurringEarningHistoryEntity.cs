using System;

namespace Cashflow.Api.Infra.Entity
{
    public class RecurringEarningHistoryEntity : BaseEntity
    {
        public long Id { get; set; }

        public decimal Value { get; set; }

        public DateTime Date { get; set; }

        public long RecurringEarningId { get; set; }
    }
}
