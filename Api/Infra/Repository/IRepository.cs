using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FinanceApi.Infra.Repository
{
  /// Contract for all repositories
  public interface IRepository<T> where T : class
  {
    /// Get by Id
    T GetById(int id);

    /// Get all entities
    List<T> GetAll();

    /// Get some entities
    List<T> GetSome(Expression<Func <T, bool>> expressions);

    /// Insert entity
    void Add(T t);

    /// Update entity
    void Update(T t);

    /// Remove entity   
    void Remove(int id);

    /// Save changes in database
    void Save();
  }
}