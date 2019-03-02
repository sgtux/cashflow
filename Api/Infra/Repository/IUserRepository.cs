using Cashflow.Api.Infra.Entity;

namespace Cashflow.Api.Infra.Repository
{
  /// User repository contract
  public interface IUserRepository : IRepository<User>
  {
    /// Verify if user exists
    bool UserExists(int userId);

    /// Find by name or email
    User FindByNameEmail(string name, string email);

    /// Find by email
    User FindByEmail(string email);
  }
}