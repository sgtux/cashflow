using System.Collections.Generic;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;

namespace Cashflow.Api.Infra.Repository
{
    public interface IVehicleRepository : IRepository<Vehicle>
    {
        Task<IEnumerable<CreditCard>> GetByUserId(int userId);
    }
}