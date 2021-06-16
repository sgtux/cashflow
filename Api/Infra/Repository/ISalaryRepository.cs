using System.Collections.Generic;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;

namespace Cashflow.Api.Infra.Repository
{
    public interface ISalaryRepository : IRepository<Salary>
    {
        Task<IEnumerable<Salary>> GetByUserId(int userId);
    }
}