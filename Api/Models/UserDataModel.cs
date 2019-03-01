using System;
using System.Collections.Generic;
using FinanceApi.Infra.Entity;

namespace Cashflow.Api.Models
{
  /// User data model
  public class UserDataModel
  {
    /// Constructor with no parameter
    public UserDataModel() { }

    /// Mapping constructor
    public UserDataModel(User user)
    {
      Id = user.Id;
      Name = user.Name;
      Email = user.Email;
    }

    /// Identifier
    public int Id { get; set; }

    /// Name
    public string Name { get; set; }

    /// Email
    public string Email { get; set; }
  }
}