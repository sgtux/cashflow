using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Resources.User;
using Cashflow.Api.Shared;

namespace Cashflow.Api.Infra.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(DatabaseContext conn) : base(conn) { }

        public Task<User> FindByEmail(string email) => FirstOrDefault(UserResources.ByEmail, new { Email = email });

        public Task<User> GetById(int id) => FirstOrDefault(UserResources.ById, new { Id = id });

        public async Task<IEnumerable<User>> GetAll() => await Query(UserResources.All);

        public Task Add(User t) => Execute(UserResources.Insert, t);

        public Task Update(User t) => throw new NotImplementedException();

        public Task Remove(int id) => throw new NotImplementedException();
    }
}