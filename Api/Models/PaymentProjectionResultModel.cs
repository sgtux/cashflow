using System.Collections.Generic;
using System.Linq;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Shared;

namespace Cashflow.Api.Models
{
    public class PaymentProjectionResultModel
    {
        public PaymentProjectionResultModel() => Payments = new List<PaymentProjectionModel>();

        public List<PaymentProjectionModel> Payments { get; }

        public decimal TotalIn => Payments.Where(p => p.Type.In && p.Description != Constants.PREVIOUS_MONTH_BALANCE).Sum(p => p.Value);

        public decimal TotalOut => Payments.Where(p => !p.Type.In && p.Description != Constants.PREVIOUS_MONTH_BALANCE).Sum(p => p.Value);

        public decimal Total => TotalIn - TotalOut;

        public decimal PreviousMonthBalanceValue
        {
            get
            {
                var payment = Payments.FirstOrDefault(p => p.Description == Constants.PREVIOUS_MONTH_BALANCE);
                if (payment == null)
                    return 0;
                return payment.Type.In ? payment.Value : payment.Value * -1;
            }
        }

        public decimal AccumulatedValue { get; set; }
    }
}