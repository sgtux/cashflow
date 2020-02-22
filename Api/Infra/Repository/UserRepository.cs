using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Cashflow.Api.Infra.Entity;
using Cashflow.Api.Shared;

namespace Cashflow.Api.Infra.Repository
{
  public class UserRepository : BaseRepository<User>, IUserRepository
  {
    public UserRepository(AppConfig config) : base(config) { }

    public bool UserExists(int userId) => userId > 0;

    public User FindByNameEmail(string name, string email) => new User();

    public User FindByEmail(string email)
    {
      return FirstOrDefault("SELECT * FROM \"User\" WHERE email = @Email", new { Email = email }).Result;
    }

    public User GetById(int id)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<User> GetAll()
    {
      throw new NotImplementedException();
    }

    public IEnumerable<User> GetSome(Expression<Func<User, bool>> expressions)
    {
      throw new NotImplementedException();
    }

    public void Add(User t)
    {
      throw new NotImplementedException();
    }

    public void Update(User t)
    {
      throw new NotImplementedException();
    }

    public void Remove(int id)
    {
      throw new NotImplementedException();
    }

    public void Save()
    {
      throw new NotImplementedException();
    }
  }
}