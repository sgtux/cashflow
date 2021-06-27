using System;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Shared;

namespace Cashflow.Api.Models
{
    public class PaymentFutureModel
    {
        public string Description { get; set; }

        public bool Invoice { get; set; }

        public int Number { get; set; }

        public DateTime? PaidDate { get; set; }

        public bool FixedPayment { get; set; }

        public int QtdInstallments { get; set; }

        public decimal Cost { get; set; }

        public TypePayment Type { get; set; }

        public CreditCard CreditCard { get; set; }

        public string MonthYear { get; set; }
    }
}