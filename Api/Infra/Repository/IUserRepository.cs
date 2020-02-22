using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;

namespace Cashflow.Api.Infra.Repository
{
  public interface IUserRepository : IRepository<User>
  {
    Task<User> FindByEmail(string email);
  }
}