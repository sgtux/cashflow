using System.Collections.Generic;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Sql.Earning;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Services;
using Cashflow.Api.Infra.Filters;
using Cashflow.Api.Contracts;

namespace Cashflow.Api.Infra.Repository
{
    public class EarningRepository : BaseRepository<EarningEntity>, IEarningRepository
    {
        public EarningRepository(IDatabaseContext conn, LogService logService) : base(conn, logService) { }

        public Task Add(EarningEntity earning) => Execute(EarningResources.Insert, earning);

        public Task<EarningEntity> GetById(long id) => FirstOrDefault(EarningResources.ById, new { Id = id });

        public Task<IEnumerable<EarningEntity>> GetSome(BaseFilter filter) => Query(EarningResources.ByUser, filter);

        public Task Remove(long id) => Execute(EarningResources.Delete, new { Id = id });

        public Task Update(EarningEntity earning) => Execute(EarningResources.Update, earning);
    }
}