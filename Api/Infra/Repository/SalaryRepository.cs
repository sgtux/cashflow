using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Sql.Salary;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Service;
using Cashflow.Api.Shared;

namespace Cashflow.Api.Infra.Repository
{
    public class SalaryRepository : BaseRepository<Salary>, ISalaryRepository
    {
        public SalaryRepository(DatabaseContext conn, LogService logService) : base(conn, logService) { }

        public Task Add(Salary salary) => Execute(SalaryResources.Insert, salary);

        public Task<IEnumerable<Salary>> GetAll() => throw new NotImplementedException();

        public Task<Salary> GetById(long id) => FirstOrDefault(SalaryResources.ById, new { Id = id });

        public Task<IEnumerable<Salary>> GetByUserId(int userId) => Query(SalaryResources.ByUser, new { UserId = userId });

        public Task<IEnumerable<Salary>> GetSome(Expression<Func<Salary, bool>> expressions) => throw new NotImplementedException();

        public Task Remove(long id) => Execute(SalaryResources.Delete, new { Id = id });

        public Task Update(Salary salary) => Execute(SalaryResources.Update, salary);
    }
}