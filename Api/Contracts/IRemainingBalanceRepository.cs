using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;

namespace Cashflow.Api.Contracts
{
    public interface IRemainingBalanceRepository : IRepository<RemainingBalance>
    {
        Task<RemainingBalance> GetByMonthYear(int userId, int month, int year);
    }
}