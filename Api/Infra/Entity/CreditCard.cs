namespace Cashflow.Api.Infra.Entity
{
    public class CreditCard
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int UserId { get; set; }

        public int InvoiceDay { get; set; }
    }
}