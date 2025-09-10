using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;

namespace Cashflow.Api.Contracts
{
    public interface IUserRepository : IRepository<UserEntity, BaseFilter>
    {
        Task<UserEntity> FindByEmail(string email);

        Task<int> TotalRegisters(int userId);
    }
}