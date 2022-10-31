using System;
using Cashflow.Api.Infra.Entity;

namespace Cashflow.Api.Models
{
    public class PaymentProjectionModel
    {
        public string Description { get; set; }

        public string Number { get; set; }

        public DateTime? PaidDate { get; set; }

        public int QtdInstallments { get; set; }

        public int QtdPaidInstallments { get; set; }

        public decimal Value { get; set; }

        public PaymentType Type { get; set; }

        public CreditCard CreditCard { get; set; }

        public string MonthYear { get; set; }

        public string TypeText => Type?.Description ?? "";

        public string CreditCardName => CreditCard?.Name ?? "";
    }
}