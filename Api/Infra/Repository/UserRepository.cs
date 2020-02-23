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
    public UserRepository(AppConfig config) : base(config) { }

    public async Task<User> FindByEmail(string email)
    {
      return await FirstOrDefault(UserResources.ByEmail, new { Email = email });
    }

    public async Task<User> GetById(int id)
    {
      return await FirstOrDefault(UserResources.ById, new { Id = id });
    }

    public async Task<IEnumerable<User>> GetAll()
    {
      return await Many(UserResources.All);
    }

    public async Task Add(User t)
    {
      await Execute(UserResources.Insert, t);
    }

    public Task Update(User t)
    {
      throw new NotImplementedException();
    }

    public Task Remove(int id)
    {
      throw new NotImplementedException();
    }
  }
}