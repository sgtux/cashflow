using System;
using System.Collections.Generic;
using System.Linq;
using Cashflow.Api.Infra.Entity;
using Microsoft.EntityFrameworkCore;

namespace Cashflow.Api.Infra.Repository
{
  /// Payment repository
  public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
  {
    /// Constructor
    public PaymentRepository(AppDbContext context) : base(context) { }

    /// Get payments by user id  
    public List<Payment> GetByUser(int userId)
    {
      return _context.Payment.Include(p => p.CreditCard).Where(p => p.UserId == userId).ToList();
    }

    /// Get current date time
    public DateTime CurrentDate => DateTime.Now;
  }
}