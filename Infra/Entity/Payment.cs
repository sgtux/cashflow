using System;
using FinanceApi.Shared;

namespace FinanceApi.Infra.Entity
{
  public class Payment
  {
    public int Id { get; set; }
    public string Description { get; set; }
    public User User { get; set; }
    public int UserId { get; set; }
    public decimal Cost { get; set; }
    public int Plots { get; set; }
    public TypePayment Type { get; set; }
    public DateTime FirstPayment { get; set; }
    public CreditCard CarditCard { get; set; }
    public CreditCard CarditCardId { get; set; }
  }
}