using System.Collections.Generic;
using System.Linq;
using FinanceApi.Shared;

namespace FinanceApi.Models
{
  public class PaymentFutureResultModel
  {
    public List<PaymentFutureModel> Payments { get; set; }
    public decimal CostIncome => Payments.Where(p => p.Type == TypePayment.Income).Sum(p => p.Cost);
    public decimal CostExpense => Payments.Where(p => p.Type == TypePayment.Expense).Sum(p => p.Cost);
    public decimal Cost => CostIncome - CostExpense;
    public decimal AcumulatedCost { get; set; }
  }
}