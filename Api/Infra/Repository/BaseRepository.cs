using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FinanceApi.Infra.Entity;
using Microsoft.EntityFrameworkCore;

namespace FinanceApi.Infra.Repository
{
  /// Base repository class
  public abstract class BaseRepository<T> : IRepository<T> where T : class
  {
    /// Context for database interactions
    protected AppDbContext _context;

    /// To facilitate database interactions
    protected DbSet<T> _dbSet;

    /// Constructor
    public BaseRepository(AppDbContext context)
    {
      _context = context;
      _dbSet = _context.Set<T>();
    }

    /// Get entity by primary key
    public T GetById(int id) => _dbSet.Find(id);

    /// Get all entities
    public virtual List<T> GetAll() => _dbSet.ToList();

    /// Get some entities
    public List<T> GetSome(Expression<Func<T, bool>> expression) => _dbSet.Where(expression).ToList();

    /// Insert entity
    public void Add(T t) => _dbSet.Add(t);

    /// Remove Entity
    public void Remove(int id) => _dbSet.Remove(GetById(id));

    /// Update entity
    public void Update(T t) => _dbSet.Update(t);

    /// Save changes in database
    public void Save() => _context.SaveChanges();
  }
}