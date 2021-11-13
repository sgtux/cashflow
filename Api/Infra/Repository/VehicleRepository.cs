using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Service;
using Cashflow.Api.Shared;
using Cashflow.Api.Infra.Sql.Vehicle;

namespace Cashflow.Api.Infra.Repository
{
    public class VehicleRepository : BaseRepository<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(DatabaseContext conn, LogService logService) : base(conn, logService) { }

        public Task Add(Vehicle vehicle) => Execute(VehicleResources.Insert, vehicle);

        public Task<Vehicle> GetById(long id) => FirstOrDefault(VehicleResources.ById, new { Id = id });

        public Task<IEnumerable<CreditCard>> GetByUserId(int userId)
        {
            throw new NotImplementedException();
        }

        public Task Remove(long id)
        {
            throw new NotImplementedException();
        }

        public Task Update(Vehicle vehicle) => Execute(VehicleResources.Update, vehicle);
    }
}