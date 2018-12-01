using System;

namespace FinanceApi.Models
{
  public class Payment
  {
    public int PaymentId { get; set; }
    public decimal Cost { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public string Place { get; set; }
    public int Plots { get; set; }
    public DateTime FirstPaymentDate { get; set; }
  }
}