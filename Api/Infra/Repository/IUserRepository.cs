using FinanceApi.Infra.Entity;

namespace FinanceApi.Infra.Repository
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