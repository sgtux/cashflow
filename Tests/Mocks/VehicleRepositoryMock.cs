using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;

namespace Cashflow.Tests.Mocks
{
    public class VehicleRepositoryMock : BaseRepositoryMock, IVehicleRepository
    {
        public Task Add(Vehicle t) => Task.Run(() => Vehicles.Add(t));

        public Task<IEnumerable<Vehicle>> GetAll() => Task.Run(() => Vehicles.AsEnumerable());

        public Task<Vehicle> GetById(long id) => Task.Run(() => Vehicles.FirstOrDefault(p => p.Id == id));

        public Task<IEnumerable<Vehicle>> GetSome(BaseFilter filter) => Task.Run(() => Vehicles.Where(p => p.UserId == filter.UserId));

        public Task Remove(long id) => Task.Run(() => Vehicles.Remove(Vehicles.FirstOrDefault(p => p.Id == id)));

        public Task Update(Vehicle t)
        {
            return Task.Run(() =>
            {
                var vehicle = Vehicles.FirstOrDefault(p => p.Id == t.Id);
                vehicle.Description = t.Description;
            });
        }

        public Task<bool> Exists(long vehicleId) => Task.Run(() => Vehicles.Any(p => p.Id == vehicleId));
    }
}