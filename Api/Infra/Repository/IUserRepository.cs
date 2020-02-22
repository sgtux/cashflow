using Cashflow.Api.Infra.Entity;

namespace Cashflow.Api.Infra.Repository
{
  public interface IUserRepository : IRepository<User>
  {
    bool UserExists(int userId);

    User FindByNameEmail(string name, string email);

    User FindByEmail(string email);
  }
}