using System.Collections.Generic;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Service;
using Cashflow.Api.Infra.Sql.Vehicle;
using System.Linq;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Filters;

namespace Cashflow.Api.Infra.Repository
{
    public class VehicleRepository : BaseRepository<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(IDatabaseContext conn, LogService logService) : base(conn, logService) { }

        public Task Add(Vehicle vehicle) => Execute(VehicleResources.Insert, vehicle);

        public async Task<Vehicle> GetById(long id)
        {
            Vehicle vehicle = null;
            await Query<FuelExpenses>(VehicleResources.ById, (x, y) =>
            {
                if (vehicle == null)
                {
                    vehicle = x;
                    vehicle.FuelExpenses = new List<FuelExpenses>();
                }
                if (y != null)
                    vehicle.FuelExpenses.Add(y);
                return x;
            }, new { Id = id });
            return vehicle;
        }

        public async Task<IEnumerable<Vehicle>> GetSome(BaseFilter filter)
        {
            var list = new List<Vehicle>();
            await Query<FuelExpenses>(VehicleResources.Some, (x, y) =>
            {
                var vehicle = list.FirstOrDefault(p => p.Id == x.Id);
                if (vehicle == null)
                {
                    vehicle = x;
                    vehicle.FuelExpenses = new List<FuelExpenses>();
                    list.Add(vehicle);
                }
                if (y != null)
                    vehicle.FuelExpenses.Add(y);
                return x;
            }, filter);
            return list;
        }

        public Task Remove(long id) => Execute(VehicleResources.Delete, new { Id = id });

        public Task Update(Vehicle vehicle) => Execute(VehicleResources.Update, vehicle);
    }
}