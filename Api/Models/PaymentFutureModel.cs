using System.Collections.Generic;
using System.Linq;
using FinanceApi.Shared;

namespace FinanceApi.Models
{
  /// Payment future model
  public class PaymentFutureModel
  {

    /// Constructor
    public PaymentFutureModel()
    {
      Items = new List<PaymentItemModel>();
    }

    /// Description
    public string Description { get; set; }

    /// Cost
    public decimal Cost => Items.Sum(p => p.Cost);

    /// Plots
    public int Plots { get; set; }

    /// Plots paid
    public int PlotsPaid { get; set; }

    /// Type
    public TypePayment Type { get; set; }

    /// Is credit card
    public bool IsCreditCard { get; set; }

    /// Month of the payments group
    public string Month { get; set; }

    /// Items
    public List<PaymentItemModel> Items { get; set; }
  }
}