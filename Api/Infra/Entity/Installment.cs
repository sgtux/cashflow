using System;

namespace Cashflow.Api.Infra.Entity
{
    public class Installment : BaseEntity
    {
        public long Id { get; set; }

        public long PaymentId { get; set; }

        public short Number { get; set; }

        public decimal Cost { get; set; }

        public DateTime Date { get; set; }

        public DateTime? PaidDate { get; set; }
    }
}