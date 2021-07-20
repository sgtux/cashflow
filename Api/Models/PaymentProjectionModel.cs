using System;
using Cashflow.Api.Infra.Entity;

namespace Cashflow.Api.Models
{
    public class PaymentProjectionModel
    {
        public string Description { get; set; }

        public bool Invoice { get; set; }

        public int Number { get; set; }

        public DateTime? PaidDate { get; set; }

        public bool Monthly { get; set; }

        public int QtdInstallments { get; set; }

        public decimal Cost { get; set; }

        public PaymentType Type { get; set; }

        public CreditCard CreditCard { get; set; }

        public string MonthYear { get; set; }
    }
}