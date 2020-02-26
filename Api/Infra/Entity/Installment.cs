namespace Cashflow.Api.Infra.Entity
{
  public class Installment
  {
    public long Id { get; set; }

    public long PaymentId { get; set; }

    public long Number { get; set; }

    public decimal Cost { get; set; }

    public System.DateTime Date { get; set; }
    
    public bool Paid { get; set; }
  }
}