using System.Collections.Generic;
using System.Linq;

namespace Cashflow.Api.Infra.Filters
{
    public class RecurringExpenseFilter : BaseFilter
    {
        public RecurringExpenseFilter() { }

        public RecurringExpenseFilter(BaseFilter filter) : base(filter) { }

        public IEnumerable<int> CreditCardIds { get; set; }

        public string CreditCardIdsStr => CreditCardIds?.Any() ?? false ? string.Join(",", CreditCardIds) : null;

        public override void FixParams()
        {
            if (CreditCardIds is not null && !CreditCardIds.Any())
                CreditCardIds = null;
        }
    }
}