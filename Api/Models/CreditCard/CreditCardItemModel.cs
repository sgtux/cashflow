using Cashflow.Api.Enums;

namespace Cashflow.Api.Models.CreditCard
{
    public class CreditCardItemModel
    {
        public long Id { get; set; }

        public CreditCardExpenseType Type { get; set; }

        public string Description { get; set; }

        public decimal OutstandingDebt { get; set; }

        public decimal CurrentMonthDebt { get; set; }

        public bool IsCurrentMonthDebtPaid { get; set; }

        public string Plots { get; set; }

        public decimal Total { get; set; }

        public bool IsInstallmentPayment => Description.EndsWith("(Parcelado)");
    }
}