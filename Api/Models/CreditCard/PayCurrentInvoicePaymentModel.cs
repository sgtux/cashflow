using Cashflow.Api.Enums;

namespace Cashflow.Api.Models.CreditCard
{
    public class PayCurrentInvoicePaymentModel
    {
        public long ItemId { get; set; }

        public decimal PaidValue { get; set; }

        public CreditCardExpenseType Type { get; set; }

        public long CreditCardId { get; set; }
    }
}