using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Service;
using Cashflow.Api.Infra.Sql.Vehicle;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Filters;

namespace Cashflow.Api.Infra.Repository
{
    public class FuelExpensesRepository : BaseRepository<FuelExpenses>, IFuelExpensesRepository
    {
        public FuelExpensesRepository(IDatabaseContext conn, LogService logService) : base(conn, logService) { }

        public Task Add(FuelExpenses t) => Execute(FuelExpensesResources.Insert, t);

        public Task<FuelExpenses> GetById(long id) => FirstOrDefault(FuelExpensesResources.ById, new { Id = id });

        public Task Update(FuelExpenses t) => Execute(FuelExpensesResources.Update, t);

        public Task Remove(long id) => Execute(FuelExpensesResources.Delete, new { Id = id });

        public Task<IEnumerable<FuelExpenses>> GetSome(BaseFilter filter) => throw new NotImplementedException();
    }
}