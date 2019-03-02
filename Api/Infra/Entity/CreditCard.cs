namespace Cashflow.Api.Infra.Entity
{
  /// Credit card
  public class CreditCard
  {
    /// Identifier
    public int Id { get; set; }
    /// Name
    public string Name { get; set; }
    /// User owner of the credit card
    public User User { get; set; }
    /// User owner identifier of the credit card
    public int UserId { get; set; }
  }
}