using Cashflow.Api.Infra.Entity;

namespace Cashflow.Api.Models
{
  public class UserDataModel
  {
    public UserDataModel() { }

    public UserDataModel(User user)
    {
      Id = user.Id;
      Name = user.Name;
      Email = user.Email;
    }

    public int Id { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }
  }
}