using System.Linq;
using Cashflow.Api.Infra.Entity;
using Microsoft.EntityFrameworkCore;

namespace Cashflow.Api.Infra
{
  /// Database context
  public class AppDbContext : DbContext
  {
    /// Constructor
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    /// User entities
    public DbSet<User> User { get; set; }

    /// Payment entities
    public DbSet<Payment> Payment { get; set; }

    /// Credit card entities
    public DbSet<CreditCard> CreditCard { get; set; }

    /// Configuration model builder
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