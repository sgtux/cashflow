using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Infra.Repository;

namespace Cashflow.Tests.Mocks
{
  public class UserRepositoryMock : BaseRepositoryMock, IUserRepository
  {
    public Task Add(User t)
    {
      return Task.Run(() => Users.Add(t));
    }

    public Task<IEnumerable<User>> GetAll() => Task.Run(() => Users.AsEnumerable());

    public Task<User> GetById(int id) => Task.Run(() => Users.FirstOrDefault(p => p.Id == id));

    public Task Remove(int id)
    {
      return Task.Run(() =>
      {
        var user = Users.FirstOrDefault(p => p.Id == id);
        if (user != null)
          Users.Remove(user);
      });
    }

    public Task Update(User t)
    {
      return Task.Run(() =>
      {
        var user = Users.FirstOrDefault(p => p.Id == t.Id);
        user.Email = t.Email;
        user.Password = t.Password;
        user.CreatedAt = t.CreatedAt;
        user.UpdatedAt = t.UpdatedAt;
      });
    }

    public Task<bool> Exists(long userId) => Task.Run(() => Users.Any(p => p.Id == userId));

    public Task<User> FindByNameEmail(string name, string email) => Task.Run(() => Users.FirstOrDefault(p => p.Email == email || p.Name == name));

    public Task<User> FindByEmail(string email) => Task.Run(() => Users.FirstOrDefault(p => p.Email == email));
  }
}