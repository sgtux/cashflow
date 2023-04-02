using System.Collections.Generic;
using System.Linq;
using Cashflow.Api.Shared;

namespace Cashflow.Api.Models
{
    public class PaymentMonthProjectionModel
    {
        public List<PaymentProjectionModel> Payments { get; set; }

        public string MonthYear { get; set; }

        public PaymentMonthProjectionModel(string monthYear, List<PaymentProjectionModel> payments)
        {
            MonthYear = monthYear;
            Payments = payments.Where(p => p.MonthYear == monthYear).ToList();
        }


        public decimal TotalIn => Payments.Where(p => p.In && p.Description != Constants.PREVIOUS_MONTH_BALANCE).Sum(p => p.Value);

        public decimal TotalOut => Payments.Where(p => !p.In && p.Description != Constants.PREVIOUS_MONTH_BALANCE).Sum(p => p.Value);

        public decimal Total => TotalIn - TotalOut;

        public decimal PreviousMonthBalanceValue
        {
            get
            {
                var payment = Payments.FirstOrDefault(p => p.Description == Constants.PREVIOUS_MONTH_BALANCE);
                if (payment == null)
                    return 0;
                return payment.In ? payment.Value : payment.Value * -1;
            }
        }

        public decimal AccumulatedValue { get; set; }
    }
}