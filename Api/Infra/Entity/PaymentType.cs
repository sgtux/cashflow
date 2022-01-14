namespace Cashflow.Api.Infra.Entity
{
    public class PaymentType : BaseEntity
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public bool In { get; set; }
    }
}