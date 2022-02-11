using System.Collections.Generic;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Sql.Earning;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Services;
using Cashflow.Api.Infra.Filters;
using Cashflow.Api.Contracts;

namespace Cashflow.Api.Infra.Repository
{
    public class EarningRepository : BaseRepository<Earning>, IEarningRepository
    {
        public EarningRepository(IDatabaseContext conn, LogService logService) : base(conn, logService) { }

        public Task Add(Earning earning) => Execute(EarningResources.Insert, earning);

        public Task<Earning> GetById(long id) => FirstOrDefault(EarningResources.ById, new { Id = id });

        public Task<IEnumerable<Earning>> GetSome(BaseFilter filter) => Query(EarningResources.ByUser, filter);

        public Task Remove(long id) => Execute(EarningResources.Delete, new { Id = id });

        public Task Update(Earning earning) => Execute(EarningResources.Update, earning);
    }
}