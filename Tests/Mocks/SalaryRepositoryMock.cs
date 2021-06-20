using System.Collections.Generic;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Repository;

namespace Cashflow.Tests.Mocks
{
    public class SalaryRepositoryMock : BaseRepositoryMock, ISalaryRepository
    {
        public Task Add(Salary t)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Exists(long userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Salary>> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Task<Salary> GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Salary>> GetByUserId(int userId)
        {
            throw new System.NotImplementedException();
        }

        public Task Remove(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task Update(Salary t)
        {
            throw new System.NotImplementedException();
        }
    }
}