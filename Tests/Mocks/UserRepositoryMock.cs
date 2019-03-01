using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FinanceApi.Infra.Entity;
using FinanceApi.Infra.Repository;

namespace Cashflow.Tests.Mocks
{
  public class UserRepositoryMock : BaseRepositoryMock, IUserRepository
  {
    public void Add(User t)
    {
      Users.Add(t);
    }

    public List<User> GetAll() => Users;

    public User GetById(int id) => Users.FirstOrDefault(p => p.Id == id);

    public List<User> GetSome(Expression<Func<User, bool>> expressions)
    {
      throw new NotImplementedException();
    }

    public void Remove(int id)
    {
      var user = Users.FirstOrDefault(p => p.Id == id);
      if (user != null)
        Users.Remove(user);
    }

    public void Save() { }

    public void Update(User t)
    {
      throw new NotImplementedException();
    }

    public bool UserExists(int userId) => Users.Any(p => p.Id == userId);
  }
}