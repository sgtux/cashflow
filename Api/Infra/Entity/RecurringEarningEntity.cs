using System;
using System.Collections.Generic;
using System.Linq;
using Cashflow.Api.Extensions;

namespace Cashflow.Api.Infra.Entity
{
    public class RecurringEarningEntity : BaseEntity
    {
        public long Id { get; set; }

        public string Description { get; set; }

        public decimal Value { get; set; }

        public DateTime? InactiveAt { get; set; }

        public int UserId { get; set; }

        public List<RecurringEarningHistoryEntity> History { get; set; }

        public bool Received => History?.Any(p => p.Date.SameMonthYear(CurrentDate)) ?? false;

        public void SortHistory()
        {
            if (HasHistory())
                History = History.OrderByDescending(p => p.Date).ToList();
        }

        public bool HasHistory() => History?.Any() ?? false;
    }
}
