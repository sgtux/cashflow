using System.Collections.Generic;

namespace FinanceApi.Models
{
  public class PaymentFutureResultModel
  {
    public decimal Cost { get; set; }
    public List<PaymentFutureModel> Payments { get; set; }
  }
}