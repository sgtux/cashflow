using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;

namespace Cashflow.Api.Contracts
{
    public interface IUserRepository : IRepository<User, BaseFilter>
    {
        Task<User> FindByEmail(string email);

        Task<int> TotalRegisters(int userId);
    }
}