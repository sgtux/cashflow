using System.Collections.Generic;
using System.Linq;
using Cashflow.Api.Shared;

namespace Cashflow.Api.Models
{
  /// Payment future model
  public class PaymentFutureResultModel
  {
    /// Payments
    public List<PaymentFutureModel> Payments { get; set; }

    /// Sum incomes
    public decimal CostIncome => Payments.Where(p => p.Type == TypePayment.Income).Sum(p => p.Cost);

    /// Sum expense
    public decimal CostExpense => Payments.Where(p => p.Type == TypePayment.Expense).Sum(p => p.Cost);

    /// Total
    public decimal Cost => CostIncome - CostExpense;

    /// Acumulated costu
    public decimal AcumulatedCost { get; set; }
  }
}