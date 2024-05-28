using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Services;
using Cashflow.Api.Infra.Sql.SystemParameter;
using Cashflow.Api.Contracts;
using System;
using Cashflow.Api.Shared;

namespace Cashflow.Api.Infra.Repository
{
    public class SystemParameterRepository : BaseRepository<SystemParameter>, ISystemParameterRepository
    {
        public SystemParameterRepository(IDatabaseContext conn, LogService logService) : base(conn, logService) { }

        public async Task<int> MaximumSystemUsers()
        {
            var param = await FirstOrDefault(SystemParameterResources.ByKey, new { Key = Constants.MAXIMUM_SYSTEM_USERS });
            return Convert.ToInt32(param.Value);
        }
    }
}