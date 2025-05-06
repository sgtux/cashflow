namespace Cashflow.Api.Models
{
    public class CreditCardItemModel
    {
        public string Description { get; set; }

        public decimal OutstandingDebt { get; set; }

        public string Plots { get; set; }

        public decimal Total { get; set; }

        public bool IsInstallmentPayment => Description.EndsWith("(Parcelado)");
    }
}