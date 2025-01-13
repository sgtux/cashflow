using System;
using Cashflow.Api.Enums;
using Cashflow.Api.Infra.Entity;

namespace Cashflow.Api.Models
{
    public class PaymentProjectionModel
    {
        public long Id { get; set; }

        public string Description { get; set; }

        public string MonthYear { get; set; }

        public decimal Value { get; set; }

        public MovementProjectionType Type { get; set; }

        public string Number { get; set; }

        public DateTime? PaidDate { get; set; }

        public int QtdInstallments { get; set; }

        public int QtdPaidInstallments { get; set; }

        public CreditCard CreditCard { get; set; }

        public PaymentProjectionModel() { }

        public PaymentProjectionModel(string description, DateTime date, decimal value, MovementProjectionType type)
        {
            Description = description;
            MonthYear = date.ToString("MM/yyyy");
            Value = value;
            Type = type;
        }

        public PaymentProjectionModel(string description, DateTime date, decimal value, MovementProjectionType type, CreditCard creditCard, long id = 0)
        {
            Description = description;
            MonthYear = date.ToString("MM/yyyy");
            Value = value;
            Type = type;
            CreditCard = creditCard;
            Id = id;
        }

        public PaymentProjectionModel(string description,
            DateTime date,
            decimal value,
            MovementProjectionType type,
            CreditCard creditCard,
            string number,
            DateTime? paidDate,
            int qtdInstallments,
            int qtdPaidInstallments)
        {
            Description = description;
            MonthYear = date.ToString("MM/yyyy");
            Value = value;
            Type = type;
            CreditCard = creditCard;
            Number = number;
            PaidDate = paidDate;
            QtdInstallments = qtdInstallments;
            QtdPaidInstallments = qtdPaidInstallments;
        }

        public bool In => Type == MovementProjectionType.Earning || (Type == MovementProjectionType.RemainingBalanceIn);

        public string CreditCardName => CreditCard?.Name ?? "";
    }
}