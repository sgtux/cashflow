namespace Cashflow.Api.Models.Home
{
    public class PendingPaymentModel
    {
        public string Description { get; set; }

        public decimal Value { get; set; }

        public bool IsInCreditCard { get; set; }

        public PendingPaymentModel(string description, decimal value, bool isInCreditCard)
        {
            Description = description;
            Value = value;
            IsInCreditCard = isInCreditCard;
        }
    }
}