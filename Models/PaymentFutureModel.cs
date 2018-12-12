using FinanceApi.Shared;

namespace FinanceApi.Models
{
  public class PaymentFutureModel
  {
    public int PaymentId { get; set; }
    public string Description { get; set; }
    public decimal Cost { get; set; }
    public decimal PlotCost { get; set; }
    public int Plots { get; set; }
    public TypePayment Type { get; set; }
    public string CreditCard { get; set; }
    public string PaymentDate { get; set; }
    public string Month { get; set; }
    public int Day { get; set; }
  }
}