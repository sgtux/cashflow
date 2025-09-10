using System.Collections.Generic;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Services;
using Cashflow.Api.Infra.Sql.Vehicle;
using System.Linq;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Filters;

namespace Cashflow.Api.Infra.Repository
{
    public class VehicleRepository : BaseRepository<VehicleEntity>, IVehicleRepository
    {
        public VehicleRepository(IDatabaseContext conn, LogService logService) : base(conn, logService) { }

        public Task Add(VehicleEntity vehicle) => Execute(VehicleResources.Insert, vehicle);

        public async Task<VehicleEntity> GetById(long id)
        {
            VehicleEntity vehicle = null;
            await Query<FuelExpenseEntity>(VehicleResources.ById, (x, y) =>
            {
                if (vehicle == null)
                {
                    vehicle = x;
                    vehicle.FuelExpenses = new List<FuelExpenseEntity>();
                }
                if (y != null)
                    vehicle.FuelExpenses.Add(y);
                return x;
            }, new { Id = id });

            return vehicle;
        }

        public async Task<IEnumerable<VehicleEntity>> GetSome(BaseFilter filter)
        {
            var list = new List<VehicleEntity>();
            await Query<FuelExpenseEntity>(VehicleResources.Some, (x, y) =>
            {
                var vehicle = list.FirstOrDefault(p => p.Id == x.Id);
                if (vehicle == null)
                {
                    vehicle = x;
                    vehicle.FuelExpenses = new List<FuelExpenseEntity>();
                    list.Add(vehicle);
                }
                if (y != null)
                    vehicle.FuelExpenses.Add(y);
                return x;
            }, filter);

            return list;
        }

        public Task Remove(long id) => Execute(VehicleResources.Delete, new { Id = id });

        public Task Update(VehicleEntity vehicle) => Execute(VehicleResources.Update, vehicle);
    }
}