using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Repository;

namespace Cashflow.Tests.Mocks
{
    public class SalaryRepositoryMock : BaseRepositoryMock, ISalaryRepository
    {
        public Task Add(Salary t) => Task.Run(() => Salaries.Add(t));

        public Task<bool> Exists(long userId) => Task.Run(() => Salaries.Any(p => p.UserId == userId));

        public Task<IEnumerable<Salary>> GetAll() => throw new System.NotImplementedException();

        public Task<Salary> GetById(int id) => Task.Run(() => Salaries.FirstOrDefault(p => p.Id == id));

        public Task<IEnumerable<Salary>> GetByUserId(int userId) => Task.Run(() => Salaries.Where(p => p.UserId == userId));

        public Task Remove(int id) => Task.Run(() => Salaries.Remove(Salaries.First(p => p.Id == id)));

        public Task Update(Salary t)
        {
            return Task.Run(() =>
            {
                Remove(t.Id);
                Salaries.Add(t);
            });
        }
    }
}