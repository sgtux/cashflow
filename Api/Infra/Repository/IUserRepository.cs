using FinanceApi.Infra.Entity;

namespace FinanceApi.Infra.Repository
{
  /// User repository contract
  public interface IUserRepository : IRepository<User>
  {
    /// Verify if user exists
    bool UserExists(int userId);
  }
}