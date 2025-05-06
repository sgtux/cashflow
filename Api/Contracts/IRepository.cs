using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Filters;

namespace Cashflow.Api.Contracts
{
    public interface IRepository<T, F>
        where T : class
        where F : BaseFilter
    {

        DateTime CurrentDate { get; }

        Task<DateTime> DbCurrentDate();

        Task<bool> Exists(long id);

        Task<int> Count();

        Task<T> GetById(long id);

        Task<IEnumerable<T>> GetSome(F filter);

        Task Add(T t);

        Task Update(T t);

        Task Remove(long id);
    }
}