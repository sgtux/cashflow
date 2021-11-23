using System.Collections.Generic;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;

namespace Cashflow.Api.Infra.Repository
{
    public interface IFuelExpensesRepository : IRepository<FuelExpenses>
    {
        Task<IEnumerable<Vehicle>> GetByVehicle(int vehicleId);
    }
}