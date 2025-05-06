using System;
using System.Collections.Generic;
using System.Linq;
using Cashflow.Api.Extensions;

namespace Cashflow.Api.Infra.Filters
{
    public class HouseholdExpenseFilter : BaseFilter
    {
        public HouseholdExpenseFilter() { }

        public HouseholdExpenseFilter(BaseFilter filter) : base(filter) { }

        public IEnumerable<int> CreditCardIds { get; set; }

        public string CreditCardIdsStr => CreditCardIds?.Any() ?? false ? string.Join(",", CreditCardIds) : null;

        public int Month { get; set; }

        public int Year { get; set; }

        public override void FixParams()
        {
            var now = CurrentDate;

            if (Month > 12 || Month < 1)
                Month = now.Month;

            if (Year > now.Year + 5 || Year < now.Year - 5)
                Year = now.Year;

            StartDate = new DateTime(Year, Month, 1).FixStartTimeFilter();
            EndDate = new DateTime(Year, Month, DateTime.DaysInMonth(Year, Month)).FixEndTimeFilter();

            if (CreditCardIds is not null && !CreditCardIds.Any())
                CreditCardIds = null;
        }
    }
}