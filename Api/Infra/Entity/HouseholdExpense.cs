using System;
using Cashflow.Api.Enums;
using Cashflow.Api.Extensions;

namespace Cashflow.Api.Infra.Entity
{
    public class HouseholdExpense : BaseEntity
    {
        public long Id { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        public int UserId { get; set; }

        public decimal Value { get; set; }

        public int? VehicleId { get; set; }

        public int? CreditCardId { get; set; }

        public CreditCard CreditCard { get; set; }

        public bool HasCreditCard => CreditCardId.HasValue;

        public string CreditCardText => CreditCard?.Name;

        public bool CurrentInvoice => HasCreditCard && Date.Day <= CreditCard.InvoiceClosingDay;

        public bool NextInvoice => HasCreditCard && Date.Day > CreditCard.InvoiceClosingDay;

        public HouseholdExpenseType Type { get; set; }

        public string TypeDescription => ((HouseholdExpenseType)Type).GetDescription();

        public bool IsRecurrent => Type != HouseholdExpenseType.DelayInterest && Type != HouseholdExpenseType.Others;
    }
}