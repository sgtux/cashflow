using System.Collections.Generic;
using System.Linq;
using FinanceApi.Shared;

namespace FinanceApi.Models
{
  public class PaymentFutureModel
  {

    public PaymentFutureModel()
    {
      Items = new List<PaymentItemModel>();
    }
    public string Description { get; set; }
    public decimal Cost => Items.Sum(p => p.Cost);
    public TypePayment Type { get; set; }
    public bool IsCreditCard { get; set; }
    public string Month { get; set; }
    public List<PaymentItemModel> Items { get; set; }
  }
}