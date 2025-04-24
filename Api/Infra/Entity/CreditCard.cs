namespace Cashflow.Api.Infra.Entity
{
    public class CreditCard : BaseEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int UserId { get; set; }

        public int InvoiceClosingDay { get; set; }

        public int InvoiceDueDay { get; set; }

        public decimal OutstandingDebt { get; set; }
    }
}