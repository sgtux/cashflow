using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Filters;

namespace Cashflow.Api.Contracts
{
    public interface IRepository<T> where T : class
    {

        DateTime CurrentDate { get; }

        Task<DateTime> DbCurrentDate();

        Task<bool> Exists(long id);

        Task<int> Count();

        Task<T> GetById(long id);

        Task<IEnumerable<T>> GetSome(BaseFilter filter);

        Task Add(T t);

        Task Update(T t);

        Task Remove(long id);
    }
}