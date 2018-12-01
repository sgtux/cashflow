using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FinanceApi.Infra.Entity;
using Microsoft.EntityFrameworkCore;

namespace FinanceApi.Infra.Repository
{
  public abstract class BaseRepository<T> : IRepository<T> where T : class
  {
    private AppDbContext _context;
    private DbSet<T> _dbSet;

    public BaseRepository(AppDbContext context)
    {
      _context = context;
      _dbSet = _context.Set<T>();
    }

    public T GetById(int id) => _dbSet.Find(id);

    public List<T> GetAll() => _dbSet.ToList();

    public List<T> GetSome(Expression<Func<T, bool>> expression) => _dbSet.Where(expression).ToList();

    public void Add(T t) => _dbSet.Add(t);

    public void Remove(int id) => _dbSet.Remove(GetById(id));

    public void Update(T t) => _dbSet.Update(t);
  }
}