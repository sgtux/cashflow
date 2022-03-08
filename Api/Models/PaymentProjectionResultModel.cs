using System.Collections.Generic;
using System.Linq;

namespace Cashflow.Api.Models
{
    public class PaymentProjectionResultModel
    {
        public PaymentProjectionResultModel() => Payments = new List<PaymentProjectionModel>();

        public List<PaymentProjectionModel> Payments { get; }

        public decimal TotalIn => Payments.Where(p => p.Type.In).Sum(p => p.Value);

        public decimal TotalOut => Payments.Where(p => !p.Type.In).Sum(p => p.Value);

        public decimal Total => TotalIn - TotalOut;

        public decimal AccumulatedValue { get; set; }
    }
}