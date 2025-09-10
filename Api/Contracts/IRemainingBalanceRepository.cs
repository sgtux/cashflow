using System;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;

namespace Cashflow.Api.Contracts
{
    public interface IRemainingBalanceRepository : IRepository<RemainingBalanceEntity, BaseFilter>
    {
        Task<RemainingBalanceEntity> GetByMonthYear(int userId, DateTime date);
    }
}