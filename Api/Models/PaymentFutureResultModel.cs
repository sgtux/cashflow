using System.Collections.Generic;
using System.Linq;
using Cashflow.Api.Shared;

namespace Cashflow.Api.Models
{
    public class PaymentFutureResultModel
    {
        public PaymentFutureResultModel() => Payments = new List<PaymentFutureModel>();

        public List<PaymentFutureModel> Payments { get; set; }

        public decimal CostIncome => Payments.Where(p => p.Type == TypePayment.Income).Sum(p => p.Cost);

        public decimal CostExpense => Payments.Where(p => p.Type == TypePayment.Expense).Sum(p => p.Cost);

        public decimal Total => CostIncome - CostExpense;

        public decimal AccumulatedCost { get; set; }
    }
}