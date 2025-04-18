using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;

namespace Cashflow.Api.Contracts
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> FindByEmail(string email);

        Task<int> TotalRegisters(int userId);
    }
}