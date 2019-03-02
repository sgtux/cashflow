using System;
using System.ComponentModel.DataAnnotations.Schema;
using Cashflow.Api.Shared;

namespace Cashflow.Api.Infra.Entity
{
  /// Payment
  public class Payment
  {
    /// Identifier
    public int Id { get; set; }

    /// Description
    public string Description { get; set; }

    /// User
    public User User { get; set; }

    /// User identifier
    public int UserId { get; set; }

    /// Cost
    public decimal Cost { get; set; }

    /// Plots
    public int Plots { get; set; }

    /// Type
    public TypePayment Type { get; set; }

    /// First payment date
    public DateTime FirstPayment { get; set; }

    /// Credit card
    public CreditCard CreditCard { get; set; }

    /// Credit card identifier
    public int? CreditCardId { get; set; }

    /// Plots paid
    public int PlotsPaid { get; set; }

    /// Is fixed payment
    public bool FixedPayment { get; set; }

    /// Is single plot
    public bool SinglePlot { get; set; }

    /// Date first payment formatted
    [NotMapped]
    public string FirstPaymentFormatted => FirstPayment.ToString("dd-MM-yyyy");
  }
}