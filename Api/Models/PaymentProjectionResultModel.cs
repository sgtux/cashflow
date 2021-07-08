using System.Collections.Generic;
using System.Linq;
using Cashflow.Api.Shared;

namespace Cashflow.Api.Models
{
    public class PaymentProjectionResultModel
    {
        public PaymentProjectionResultModel() => Payments = new List<PaymentProjectionModel>();

        public List<PaymentProjectionModel> Payments { get; set; }

        public decimal CostExpense => Payments.Where(p => p.Type.Id == (int)PaymentTypeEnum.Expense).Sum(p => p.Cost);

        public decimal CostGain => Payments.Where(p => p.Type.Id == (int)PaymentTypeEnum.Gain).Sum(p => p.Cost);

        public decimal CostDividends => Payments.Where(p => p.Type.Id == (int)PaymentTypeEnum.Dividends).Sum(p => p.Cost);

        public decimal CostContributions => Payments.Where(p => p.Type.Id == (int)PaymentTypeEnum.Contributions).Sum(p => p.Cost);

        public decimal Total => (CostDividends + CostGain) - (CostExpense - CostContributions);

        public decimal AccumulatedCost { get; set; }
    }
}