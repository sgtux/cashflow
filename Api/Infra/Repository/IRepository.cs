using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cashflow.Api.Infra.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<bool> Exists(long id);

        Task<T> GetById(long id);

        Task Add(T t);

        Task Update(T t);

        Task Remove(long id);
    }
}