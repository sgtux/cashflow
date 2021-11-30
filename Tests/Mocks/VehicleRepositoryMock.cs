using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Repository;

namespace Cashflow.Tests.Mocks
{
    public class VehicleRepositoryMock : BaseRepositoryMock, IVehicleRepository
    {
        public Task Add(Vehicle t) => Task.Run(() => Vehicles.Add(t));

        public Task<IEnumerable<Vehicle>> GetAll() => Task.Run(() => Vehicles.AsEnumerable());

        public Task<Vehicle> GetById(long id) => Task.Run(() => Vehicles.FirstOrDefault(p => p.Id == id));

        public Task<IEnumerable<Vehicle>> GetByUserId(int userId) => Task.Run(() => Vehicles.Where(p => p.UserId == userId));

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