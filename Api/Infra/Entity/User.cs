using System;
using System.Collections.Generic;

namespace Cashflow.Api.Infra.Entity
{
  /// User
  public class User
  {
    /// Identifier
    public int Id { get; set; }

    /// Name
    public string Name { get; set; }

    /// Email
    public string Email { get; set; }

    /// Password
    public string Password { get; set; }

    /// Created at date
    public DateTime CreatedAt { get; set; }

    /// Updated at date
    public DateTime? UpdatedAt { get; set; }

    /// Last access date
    public DateTime LastAccess { get; set; }

    /// Payments of the user
    public ICollection<Payment> Payments { get; set; }
  }
}