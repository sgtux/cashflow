namespace Cashflow.Api.Models.Home
{
    public class PendingPaymentModel
    {
        public string Description { get; set; }

        public decimal Value { get; set; }

        public PendingPaymentModel(string description, decimal value)
        {
            Description = description;
            Value = value;
        }
    }
}