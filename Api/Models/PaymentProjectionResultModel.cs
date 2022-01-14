using System.Collections.Generic;
using System.Linq;
using Cashflow.Api.Enums;

namespace Cashflow.Api.Models
{
    public class PaymentProjectionResultModel
    {
        public PaymentProjectionResultModel() => Payments = new List<PaymentProjectionModel>();

        public List<PaymentProjectionModel> Payments { get; }

        public decimal TotalIn => Payments.Where(p => p.Type.In).Sum(p => p.Cost);

        public decimal TotalOut => Payments.Where(p => !p.Type.In).Sum(p => p.Cost);

        public decimal Total => TotalIn - TotalOut;

        public decimal AccumulatedCost { get; set; }
    }
}