using System.Collections.Generic;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Sql.Salary;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Services;
using Cashflow.Api.Infra.Filters;
using Cashflow.Api.Contracts;

namespace Cashflow.Api.Infra.Repository
{
    public class SalaryRepository : BaseRepository<Salary>, ISalaryRepository
    {
        public SalaryRepository(IDatabaseContext conn, LogService logService) : base(conn, logService) { }

        public Task Add(Salary salary) => Execute(SalaryResources.Insert, salary);

        public Task<Salary> GetById(long id) => FirstOrDefault(SalaryResources.ById, new { Id = id });

        public Task<IEnumerable<Salary>> GetSome(BaseFilter filter) => Query(SalaryResources.ByUser, filter);

        public Task Remove(long id) => Execute(SalaryResources.Delete, new { Id = id });

        public Task Update(Salary salary) => Execute(SalaryResources.Update, salary);
    }
}