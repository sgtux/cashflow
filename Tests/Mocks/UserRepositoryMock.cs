using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Contracts;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Filters;

namespace Cashflow.Tests.Mocks
{
    public class UserRepositoryMock : BaseRepositoryMock, IUserRepository
    {
        public Task Add(User t) => Task.Run(() => Users.Add(t));

        public Task<IEnumerable<User>> GetAll() => Task.Run(() => Users.AsEnumerable());

        public Task<User> GetById(long id) => Task.Run(() => Users.FirstOrDefault(p => p.Id == id));

        public Task Remove(long id) => Task.Run(() => Users.Remove(Users.FirstOrDefault(p => p.Id == id)));

        public Task Update(User t)
        {
            return Task.Run(() =>
            {
                var user = Users.FirstOrDefault(p => p.Id == t.Id);
                user.Password = t.Password;
                user.CreatedAt = t.CreatedAt;
            });
        }

        public Task<bool> Exists(long userId) => Task.Run(() => Users.Any(p => p.Id == userId));

        public Task<User> FindByNickName(string nickName) => Task.Run(() => Users.FirstOrDefault(p => p.NickName == nickName));

        public Task<IEnumerable<User>> GetSome(BaseFilter filter) => throw new System.NotImplementedException();
    }
}