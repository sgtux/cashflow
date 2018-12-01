using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FinanceApi.Infra.Repository
{
  public interface IRepository<T> where T : class
  {
    T GetById(int id);
    List<T> GetAll();
    List<T> GetSome(Expression<Func <T, bool>> expressions);
    void Add(T t);
    void Update(T t);
    void Remove(int id);
  }
}