using System;
using System.ComponentModel.DataAnnotations.Schema;
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
    public CreditCard CreditCard { get; set; }
    public int? CreditCardId { get; set; }
    public int PlotsPaid { get; set; }
    public bool FixedPayment { get; set; }
    [NotMapped]
    public string FirstPaymentFormatted => FirstPayment.ToString("dd-MM-yyyy");
  }
}