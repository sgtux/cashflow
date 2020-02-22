using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cashflow.Api.Infra.Repository
{
  public interface IRepository<T> where T : class
  {
    Task<bool> Exists(long userId);
    
    Task<T> GetById(int id);

    Task<IEnumerable<T>> GetAll();

    Task Add(T t);

    Task Update(T t);

    Task Remove(int id);
  }
}