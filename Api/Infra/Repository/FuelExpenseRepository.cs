using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Services;
using Cashflow.Api.Infra.Sql.Vehicle;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Filters;

namespace Cashflow.Api.Infra.Repository
{
    public class FuelExpenseRepository : BaseRepository<FuelExpenseEntity>, IFuelExpenseRepository
    {
        public FuelExpenseRepository(IDatabaseContext conn,
            LogService logService) : base(conn, logService) { }

        public Task Add(FuelExpenseEntity t) => Execute(FuelExpenseResources.Insert, t);

        public async Task<FuelExpenseEntity> GetById(long id) => await FirstOrDefault(FuelExpenseResources.ById, new { Id = id });

        public Task Update(FuelExpenseEntity t) => Execute(FuelExpenseResources.Update, t);

        public Task Remove(long id) => Execute(FuelExpenseResources.Delete, new { Id = id });

        public Task<IEnumerable<FuelExpenseEntity>> GetSome(BaseFilter filter) => throw new NotImplementedException();
    }
}