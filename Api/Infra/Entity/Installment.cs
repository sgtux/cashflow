using System;

namespace Cashflow.Api.Infra.Entity
{
    public class Installment
    {
        public long Id { get; set; }

        public long PaymentId { get; set; }

        public int Number { get; set; }

        public decimal Cost { get; set; }

        public DateTime Date { get; set; }

        public DateTime? PaidDate { get; set; }
    }
}