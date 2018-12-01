using System.Linq;
using FinanceApi.Infra.Entity;
using Microsoft.EntityFrameworkCore;

namespace FinanceApi.Infra
{
  public class AppDbContext : DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> User { get; set; }
    public DbSet<Payment> Payment { get; set; }
    public DbSet<CreditCard> CreditCard { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      var props = builder.Model.GetEntityTypes()
        .SelectMany(p => p.GetProperties())
        .Where(p => p.ClrType == typeof(string));
        
      foreach (var p in props)
        if (p.GetMaxLength() is null)
          p.SetMaxLength(256);
      builder.Entity<User>().HasKey(p => p.Id);
      builder.Entity<Payment>().HasKey(p => p.Id);
      builder.Entity<CreditCard>().HasKey(p => p.Id);
      base.OnModelCreating(builder);
    }
  }
}