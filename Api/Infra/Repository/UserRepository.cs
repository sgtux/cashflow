using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;
using Cashflow.Api.Infra.Sql.User;
using Cashflow.Api.Services;

namespace Cashflow.Api.Infra.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IDatabaseContext conn, LogService logService) : base(conn, logService) { }

        public Task<User> FindByNickName(string nickName) => FirstOrDefault(UserResources.ByNickName, new { NickName = nickName });

        public Task<User> GetById(long id) => FirstOrDefault(UserResources.ById, new { Id = id });

        public Task Add(User t) => Execute(UserResources.Insert, t);

        public Task Update(User t) => throw new NotImplementedException();

        public Task UpdateSpendingCeiling(int userId, decimal spendingCeiling) => Execute(UserResources.UpdateSpendingCeiling, new { Id = userId, SpendingCeiling = spendingCeiling });

        public Task Remove(long id) => throw new NotImplementedException();

        public Task<IEnumerable<User>> GetSome(BaseFilter filter) => throw new NotImplementedException();
    }
}