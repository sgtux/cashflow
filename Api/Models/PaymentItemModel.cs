using FinanceApi.Shared;

namespace FinanceApi.Models
{
  /// Payment item model
  public class PaymentItemModel
  {
    /// Payment identifier
    public int PaymentId { get; set; }

    /// Description
    public string Description { get; set; }

    /// Cost
    public decimal Cost { get; set; }

    /// Plots
    public int Plots { get; set; }

    /// Plots paid
    public int PlotsPaid { get; set; }

    /// Type
    public TypePayment Type { get; set; }

    /// Credit card name
    public string CreditCard { get; set; }

    /// Pay date
    public string PaymentDate { get; set; }

    /// Month
    public string Month { get; set; }

    /// Pay day
    public int Day { get; set; }
  }
}