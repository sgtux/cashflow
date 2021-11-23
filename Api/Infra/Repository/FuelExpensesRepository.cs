using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Service;
using Cashflow.Api.Shared;
using Cashflow.Api.Infra.Sql.Vehicle;

namespace Cashflow.Api.Infra.Repository
{
    public class FuelExpensesRepository : BaseRepository<FuelExpenses>, IFuelExpensesRepository
    {
        public FuelExpensesRepository(DatabaseContext conn, LogService logService) : base(conn, logService) { }

        public Task Add(FuelExpenses t) => Execute(FuelExpensesResources.Insert, t);

        public Task<FuelExpenses> GetById(long id) => FirstOrDefault(FuelExpensesResources.ById, new { Id = id });

        public Task<IEnumerable<Vehicle>> GetByVehicle(int vehicleId)
        {
            throw new NotImplementedException();
        }

        public Task Update(FuelExpenses t) => Execute(FuelExpensesResources.Update, t);

        public Task Remove(long id) => Execute(FuelExpensesResources.Delete, new { Id = id });
    }
}