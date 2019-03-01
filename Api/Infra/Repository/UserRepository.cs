using System.Linq;

namespace FinanceApi.Infra.Repository
{
  /// User repository
  public class UserRepository : BaseRepository<Entity.User>, IUserRepository
  {
    /// Constructor
    public UserRepository(AppDbContext context) : base(context) { }

    /// Verify if user exists
    public bool UserExists(int userId) => _context.User.Any(p => p.Id == userId);
  }
}