using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Sql.CreditCard;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Services;
using Cashflow.Api.Infra.Filters;
using Cashflow.Api.Contracts;

namespace Cashflow.Api.Infra.Repository
{
    public class RemainingBalanceRepository : BaseRepository<RemainingBalance>, IRemainingBalanceRepository
    {
        public RemainingBalanceRepository(IDatabaseContext conn, LogService logService) : base(conn, logService) { }

        public Task Add(RemainingBalance remainingBalance) => Execute(RemainingBalanceResources.Insert, remainingBalance);

        public Task<RemainingBalance> GetById(long id) => throw new NotImplementedException();

        public Task<RemainingBalance> GetByMonthYear(int userId, int month, int year)
        => FirstOrDefault(RemainingBalanceResources.ByMonthYear, new
        {
            UserId = userId,
            Month = month,
            Year = year
        });

        public Task<IEnumerable<RemainingBalance>> GetSome(BaseFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task Remove(long id) => Execute(RemainingBalanceResources.Delete, new { Id = id });

        public Task Update(RemainingBalance remainingBalance) => Execute(RemainingBalanceResources.Update, remainingBalance);
    }
}