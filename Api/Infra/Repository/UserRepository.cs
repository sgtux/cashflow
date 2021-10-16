using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Sql.User;
using Cashflow.Api.Service;
using Cashflow.Api.Shared;

namespace Cashflow.Api.Infra.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(DatabaseContext conn, LogService logService) : base(conn, logService) { }

        public Task<User> FindByNickName(string nickName) => FirstOrDefault(UserResources.ByNickName, new { NickName = nickName });

        public Task<User> GetById(long id) => FirstOrDefault(UserResources.ById, new { Id = id });

        public async Task<IEnumerable<User>> GetAll() => await Query(UserResources.All);

        public Task Add(User t) => Execute(UserResources.Insert, t);

        public Task Update(User t) => throw new NotImplementedException();

        public Task Remove(long id) => throw new NotImplementedException();
    }
}