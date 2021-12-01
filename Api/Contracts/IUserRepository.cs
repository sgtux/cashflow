using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;

namespace Cashflow.Api.Contracts
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> FindByNickName(string nickName);
    }
}