using System;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;

namespace Cashflow.Api.Contracts
{
    public interface IRemainingBalanceRepository : IRepository<RemainingBalance, BaseFilter>
    {
        Task<RemainingBalance> GetByMonthYear(int userId, DateTime date);
    }
}