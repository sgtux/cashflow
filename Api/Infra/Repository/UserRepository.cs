using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Infra.Resources.User;
using Cashflow.Api.Infra.Entity;
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

    public Task Add(User t)
    {
      throw new NotImplementedException();
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